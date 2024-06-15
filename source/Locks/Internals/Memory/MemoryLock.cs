using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Locks.Internals.Memory
{
    internal sealed class MemoryLock : IMemoryLock
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public async Task<IMemoryLockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            var @lock = _locks.GetOrAdd(key, x => new SemaphoreSlim(1, 1));

            await @lock.WaitAsync(cancellationToken).ConfigureAwait(false);

            return new MemoryLockInstance(@lock);
        }
    }
}
