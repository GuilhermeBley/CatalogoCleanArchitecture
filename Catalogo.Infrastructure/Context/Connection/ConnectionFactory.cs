using System.Data.Common;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Catalogo.Infrastructure.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        /// <summary>
        /// Acess connection string
        /// </summary>
        private const string KeyConnectionString = "DefaultConnectionCatalogoDapper";

        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection CreateConn()
        {
            return 
                new MySqlConnection(
                    _configuration.GetConnectionString(KeyConnectionString)
                );
        }
    }
}