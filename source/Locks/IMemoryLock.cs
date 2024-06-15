using System.Threading.Tasks;
using System.Threading;

namespace Locks
{
    public interface IMemoryLock
    {
        Task<IMemoryLockInstance> Acquire(string key, CancellationToken cancellationToken = default);
    }
}
