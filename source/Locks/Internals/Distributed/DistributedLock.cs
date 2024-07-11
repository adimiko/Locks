using Locks.Internals.Distributed.Storage;
using Locks.Internals.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Locks.Internals.Distributed
{
    internal sealed class DistributedLock : IDistributedLock
    {
        private static ConcurrentDictionary<string, bool> _isKeyAdded = new ConcurrentDictionary<string, bool>();

        private readonly IDistributedLockRepository _repo;

        private readonly IDistributedLockSettings _settings;

        public DistributedLock(
            IDistributedLockRepository repo,
            IDistributedLockSettings settings)
        {
            _repo = repo;
            _settings = settings;
        }

        public async Task<IDistributedLockInstance> AcquireAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            IMemoryLockInstance memoryLockInstance = null;

            try
            {
                var isAddedFirstTime = false;

                _ = _isKeyAdded.GetOrAdd(key, x =>
                {
                    isAddedFirstTime = true;

                    return isAddedFirstTime;
                });

                if (isAddedFirstTime)
                {
                    await AddFirstLock(key).ConfigureAwait(false);
                }

                memoryLockInstance = await new MemoryLock()
                .AcquireAsync(key, cancellationToken)
                .ConfigureAwait(false);

                bool isAcquired = false;

                DateTime expirationUtc;

                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var nowUtc = DateTime.UtcNow;

                    expirationUtc = nowUtc + _settings.LockTimeout;

                    isAcquired = await _repo.TryAcquire(key, nowUtc, expirationUtc).ConfigureAwait(false);

                    if (!isAcquired)
                    {
                        await Task.Delay(_settings.CheckingIntervalWhenLockIsNotReleased, cancellationToken).ConfigureAwait(false);
                    }
                }
                while (!isAcquired);

                cancellationToken.ThrowIfCancellationRequested();

                var @lock = new DistributedLockStorageModel(key, expirationUtc);

                return new DistributedLockInstance(memoryLockInstance, @lock, _repo);
            }
            catch
            {
                if (memoryLockInstance != null) 
                {
                    memoryLockInstance.Dispose();
                }
                
                throw;
            }
        }

        private async Task AddFirstLock(string key)
        {
            var nowUtc = DateTime.UtcNow;

            var @lock = new DistributedLockStorageModel(key, nowUtc);

            await _repo.AddFirstLock(@lock).ConfigureAwait(false);
        }
    }
}
