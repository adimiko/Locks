using System.Threading;

namespace Locks.Internals.Memory
{
    internal sealed class MemoryLockInstance : IMemoryLockInstance
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        internal MemoryLockInstance(SemaphoreSlim semaphoreSlim) => _semaphoreSlim = semaphoreSlim;

        public void Dispose() => _semaphoreSlim.Release();
    }
}
