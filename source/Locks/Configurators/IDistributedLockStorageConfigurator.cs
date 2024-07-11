using Microsoft.Extensions.DependencyInjection;

namespace Locks.Configurators
{
    public interface IDistributedLockStorageConfigurator
    {
        IServiceCollection Services { get; }
    }
}
