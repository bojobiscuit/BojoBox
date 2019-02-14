using System;

namespace BojoBox.EntityFramework.Connection
{
    public class ConnectionString : IConnectionString
    {
        public ConnectionString()
        {
            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            var password = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD") ?? "Passw0rd123";
            _connectionString = $"Data Source={hostname};Database=bojoboxdb;Initial Catalog=bojoboxdb;User ID=sa;Password={password};";
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        private string _connectionString;
    }
}
