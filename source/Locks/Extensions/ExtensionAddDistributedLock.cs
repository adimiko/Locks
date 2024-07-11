using Locks.Configurators;
using Locks.Internals.Distributed;
using Locks.Internals.Distributed.Configurators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Locks
{
    public static class ExtensionAddDistributedLock
    {
        public static IServiceCollection AddDistributedLock(
            this IServiceCollection services,
            Action<IDistributedLockStorageConfigurator> storageConfiguration)
        {
            var storage = new DistributedLockStorageConfigurator(services);

            storageConfiguration(storage);

            services.AddSingleton<IDistributedLock, DistributedLock>();

            return services;
        }
    }
}
