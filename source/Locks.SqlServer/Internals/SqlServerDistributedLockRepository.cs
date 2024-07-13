using System;
using System.Threading.Tasks;
using Locks.Internals.Distributed.Storage;
using Microsoft.Data.SqlClient;

namespace Locks.SqlServer.Internals
{
    internal sealed class SqlServerDistributedLockRepository : IDistributedLockRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public SqlServerDistributedLockRepository(SqlServerConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddFirstLock(DistributedLockStorageModel @lock)
        {
            const string queryString = "INSERT INTO [dbo].[DistributedLocks] VALUES (@Key, @ExpirationUtc)";

            using (var connection = _connectionFactory.Create())
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add(new SqlParameter("ExpirationUtc", @lock.ExpirationUtc));
                command.Parameters.Add(new SqlParameter("Key", @lock.Key));

                connection.Open();

                await command
                    .ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<bool> Release(DistributedLockStorageModel @lock, DateTime nowUtc)
        {
            const string queryString = "UPDATE [dbo].[DistributedLocks] SET ExpirationUtc = @NowUtc WHERE [Key] = @Key AND ExpirationUtc = @ExpirationUtc";

            int updatedRows = 0;

            using (var connection = _connectionFactory.Create())
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add(new SqlParameter("NowUtc", nowUtc));
                command.Parameters.Add(new SqlParameter("ExpirationUtc", @lock.ExpirationUtc));
                command.Parameters.Add(new SqlParameter("Key", @lock.Key));


                connection.Open();

                updatedRows = await command
                    .ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }

            return updatedRows > 0;
        }

        public async Task<bool> TryAcquire(string key, DateTime nowUtc, DateTime newExpirationUtc)
        {
            const string queryString = "UPDATE [dbo].[DistributedLocks] SET ExpirationUtc = @ExpirationUtc WHERE [Key] = @Key AND ExpirationUtc <= @NowUtc";

            int updatedRows = 0;

            using (var connection = _connectionFactory.Create())
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add(new SqlParameter("ExpirationUtc", newExpirationUtc));
                command.Parameters.Add(new SqlParameter("Key", key));
                command.Parameters.Add(new SqlParameter("NowUtc", nowUtc));

                connection.Open();

                updatedRows = await command
                    .ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }

            return updatedRows > 0;
        }
    }
}
