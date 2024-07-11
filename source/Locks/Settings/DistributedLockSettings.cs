using Locks.Internals.Distributed;
using System;

namespace Locks.Settings
{
    public sealed class DistributedLockSettings : IDistributedLockSettings
    {
        public TimeSpan LockTimeout { get; set; } =  TimeSpan.FromSeconds(10);

        public TimeSpan CheckingIntervalWhenLockIsNotReleased { get; set; } = TimeSpan.FromMilliseconds(50);

        internal DistributedLockSettings() { }
    }
}
