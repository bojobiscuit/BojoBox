using System;

namespace BojoBox.EntityFramework.Connection
{
    public class TestConnectionString : IConnectionString
    {
        public TestConnectionString()
        {
            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            var password = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD") ?? "Passw0rd123";
            _connectionString = $"Data Source={hostname};Database=startnetdb;Initial Catalog=reviewdbtest;User ID=sa;Password={password};";
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        private string _connectionString;
    }
}
