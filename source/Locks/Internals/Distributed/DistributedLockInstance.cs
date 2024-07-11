using Locks.Internals.Distributed.Storage;
using System;
using System.Threading.Tasks;

namespace Locks.Internals.Distributed
{
    internal sealed class DistributedLockInstance : IDistributedLockInstance
    {
        private readonly IMemoryLockInstance _memoryLockInstance;

        private readonly DistributedLockStorageModel _distributedLockStorageModel;

        private readonly IDistributedLockRepository _repo;

        internal DistributedLockInstance(
            IMemoryLockInstance memoryLockInstance,
            DistributedLockStorageModel distributedLockStorageModel,
            IDistributedLockRepository repo) 
        { 
            _memoryLockInstance = memoryLockInstance;
            _distributedLockStorageModel = distributedLockStorageModel;
            _repo = repo;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                var nowUtc = DateTime.UtcNow;

                var x = await _repo.Release(_distributedLockStorageModel, nowUtc).ConfigureAwait(false);

                //TODO Log Warning (x)
            }
            finally
            {
                if (_memoryLockInstance != null)
                {
                   _memoryLockInstance.Dispose();
                }
            }
        }
    }
}
