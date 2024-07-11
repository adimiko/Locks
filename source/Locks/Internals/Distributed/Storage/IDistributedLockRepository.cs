using System;
using System.Threading.Tasks;

namespace Locks.Internals.Distributed.Storage
{
    internal interface IDistributedLockRepository
    {
        Task AddFirstLock(DistributedLockStorageModel @lock);

        Task<bool> TryAcquire(string key, DateTime nowUtc, DateTime newExpirationUtc);

        Task<bool> Release(DistributedLockStorageModel @lock, DateTime nowUtc);
    }
}
