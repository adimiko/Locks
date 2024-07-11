using Locks.Tests.Seed;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace Locks.Tests
{
    public sealed class DistributedLockTests : IAsyncLifetime
    {
        private IServiceProvider _serviceProvider;

        public async Task InitializeAsync()
        {
            var msSqlContainer = new MsSqlBuilder().Build();

            await msSqlContainer.StartAsync();

            var connectionString = msSqlContainer.GetConnectionString();

            IServiceCollection services = new ServiceCollection();

            services.AddDistributedLock(x => x.UseSqlServer(connectionString));

            // KeyedInMemoryLock is disabled because we want multiple tasks to query the database
            services.AddSingleton<IMemoryLock, DisabledMemoryLock>();

            _serviceProvider = services.BuildServiceProvider();

            const string queryStringCreate = "CREATE TABLE [dbo].[DistributedLocks] ([Key] VARCHAR(60) NOT NULL PRIMARY KEY, [ExpirationUtc] DATETIME NOT NULL)";

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryStringCreate, connection);

                connection.Open();

                await command
                    .ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

        [Fact(DisplayName = "Distributed lock works correct in distributed enviroment")]
        public async Task Test()
        {
            var distributedLock = _serviceProvider.GetRequiredService<IDistributedLock>();

            const string lockKeyA = "A";
            const string lockKeyB = "B";
            const string lockKeyC = "C";

            for (var i = 0; i < 2; i++)
            {
                var a1 = distributedLock.AcquireAsync(lockKeyA);
                var b1 = distributedLock.AcquireAsync(lockKeyB);
                var c1 = distributedLock.AcquireAsync(lockKeyC);

                await Task.WhenAll(a1, b1, c1);

                var a2 = distributedLock.AcquireAsync(lockKeyA);
                var b2 = distributedLock.AcquireAsync(lockKeyB);
                var c2 = distributedLock.AcquireAsync(lockKeyC);

                Assert.True(a1.IsCompleted, Message(nameof(a1)));
                Assert.True(b1.IsCompleted, Message(nameof(b1)));
                Assert.True(c1.IsCompleted, Message(nameof(c1)));

                await Task.Delay(250);

                // Second tasks should wait when locks will be released
                Assert.False(a2.IsCompleted, Message(nameof(a2), nameof(a1)));
                Assert.False(b2.IsCompleted, Message(nameof(b2), nameof(b1)));
                Assert.False(c2.IsCompleted, Message(nameof(c2), nameof(c1)));

                // When first tasks release locks, second tasks can continue
                await a1.Result.DisposeAsync();
                await b1.Result.DisposeAsync();
                await c1.Result.DisposeAsync();

                await Task.WhenAll(a2, b2, c2);

                Assert.True(a2.IsCompleted, Message(nameof(a2)));
                Assert.True(b2.IsCompleted, Message(nameof(b2)));
                Assert.True(c2.IsCompleted, Message(nameof(c2)));

                await a2.Result.DisposeAsync();
                await b2.Result.DisposeAsync();
                await c2.Result.DisposeAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private string Message(string x) => $"Task {x} should be completed.";

        private string Message(string x, string y) => $"Task {x} did not wait for task {y} to release the distributed lock.";
    }
}
