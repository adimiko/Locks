using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Locks.SqlServer.Internals
{
    internal sealed class SqlServerConnectionFactory
    {
        private readonly string _connectionString;

        internal SqlServerConnectionFactory(string connectionString) => _connectionString = connectionString;

        public SqlConnection Create() => new SqlConnection(_connectionString);
    }
}
