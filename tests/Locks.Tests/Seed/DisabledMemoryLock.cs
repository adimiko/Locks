namespace Locks.Tests.Seed
{
    internal sealed class DisabledMemoryLock : IMemoryLock
    {
        public Task<IMemoryLockInstance> AcquireAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IMemoryLockInstance>(new DisabledMemoryLockInstance());
        }
    }
}
