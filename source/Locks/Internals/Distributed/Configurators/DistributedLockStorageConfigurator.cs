using Locks.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Locks.Internals.Distributed.Configurators
{
    internal sealed class DistributedLockStorageConfigurator : IDistributedLockStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal DistributedLockStorageConfigurator(IServiceCollection services) 
        {
            Services = services;
        }
    }
}
