using System.Threading.Tasks;
using System.Threading;

namespace Locks
{
    public interface IMemoryLock
    {
        Task<IMemoryLockInstance> AcquireAsync(string key, CancellationToken cancellationToken = default);
    }
}
