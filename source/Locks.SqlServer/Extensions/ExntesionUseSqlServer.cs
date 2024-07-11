using Locks.Configurators;

namespace Locks.SqlServer.Extensions
{
    public static class ExntesionUseSqlServer
    {
        public static void UseSqlServer(
            this IDistributedLockStorageConfigurator configurator)
        {
            var services = configurator.Services;
        }
    }
}
