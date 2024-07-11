using System;

namespace Locks.Internals.Distributed.Storage
{
    internal sealed class DistributedLockStorageModel
    {
        public string Key { get; }

        public DateTime ExpirationUtc { get; }

        internal DistributedLockStorageModel(string key, DateTime expirationUtc)
        {
            Key = key;
            ExpirationUtc = expirationUtc;
        }
    }
}
