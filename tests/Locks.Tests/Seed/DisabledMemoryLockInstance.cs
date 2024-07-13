namespace Locks.Tests.Seed
{
    internal sealed class DisabledMemoryLockInstance : IMemoryLockInstance
    {
        public void Dispose() { }
    }
}
