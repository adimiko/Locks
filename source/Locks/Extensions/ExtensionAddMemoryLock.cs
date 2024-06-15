using Locks.Internals.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Locks
{
    public static class ExtensionAddMemoryLock
    {
        public static IServiceCollection AddMemoryLock(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryLock, MemoryLock>();

            return services;
        }
    }
}
