using Locks;
using Microsoft.Extensions.DependencyInjection;


IServiceCollection services = new ServiceCollection();

services.AddMemoryLock();

var sp = services.BuildServiceProvider();

using (var scope = sp.CreateScope())
{
    var memoryLock = scope.ServiceProvider.GetRequiredService<IMemoryLock>();

    using (memoryLock.AcquireAsync("YOUR_KEY"))
    {
        Console.WriteLine("YOUR_LOGIC");
    }
}