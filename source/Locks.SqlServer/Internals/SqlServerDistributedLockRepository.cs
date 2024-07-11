using System;
using System.Threading.Tasks;
using Locks.Internals.Distributed.Storage;

namespace Locks.SqlServer.Internals
{
    internal sealed class SqlServerDistributedLockRepository : IDistributedLockRepository
    {
        public Task AddFirstLock(DistributedLockStorageModel @lock)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Release(DistributedLockStorageModel @lock, DateTime nowUtc)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryAcquire(string key, DateTime nowUtc, DateTime newExpirationUtc)
        {
            throw new NotImplementedException();
        }
    }
}
