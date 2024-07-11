using System;

namespace Locks.Internals.Distributed
{
    internal interface IDistributedLockSettings
    {
        TimeSpan LockTimeout { get; }

        TimeSpan CheckingIntervalWhenLockIsNotReleased { get; }
    }
}
