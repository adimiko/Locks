using Locks.Configurators;
using Locks.Internals.Distributed.Storage;
using Locks.SqlServer.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace Locks.SqlServer.Extensions
{
    public static class ExntesionUseSqlServer
    {
        public static void UseSqlServer(
            this IDistributedLockStorageConfigurator configurator,
            string connectionString)
        {
            var services = configurator.Services;

            services.AddSingleton(new SqlServerConnectionFactory(connectionString));
            services.AddSingleton<IDistributedLockRepository, SqlServerDistributedLockRepository>();
        }
    }
}
