using System.Threading.Tasks;
using System.Threading;

namespace Locks
{
    public interface IDistributedLock
    {
        Task<IDistributedLockInstance> AcquireAsync(string key, CancellationToken cancellationToken = default);
    }
}
