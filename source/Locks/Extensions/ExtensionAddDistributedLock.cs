using System;
using Microsoft.Extensions.DependencyInjection;
using Locks.Configurators;
using Locks.Internals.Distributed;
using Locks.Internals.Distributed.Configurators;
using Locks.Settings;

namespace Locks
{
    public static class ExtensionAddDistributedLock
    {
        public static IServiceCollection AddDistributedLock(
            this IServiceCollection services,
            Action<IDistributedLockStorageConfigurator> storageConfiguration,
            Action<DistributedLockSettings> settingsConfiguration = null)
        {
            var storage = new DistributedLockStorageConfigurator(services);
            var settings = new DistributedLockSettings();

            storageConfiguration(storage);

            if (settingsConfiguration != null) 
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton<IDistributedLockSettings>(settings);
            services.AddSingleton<IDistributedLock, DistributedLock>();

            return services;
        }
    }
}
