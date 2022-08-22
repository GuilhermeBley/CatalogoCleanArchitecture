using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Catalogo.Infrastructure.Connection
{
    internal class ConnectionFactory : IConnectionFactory
    {
        /// <summary>
        /// Acess connection string
        /// </summary>
        private const string KeyConnectionString = "";

        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection CreateConn()
        {
            return null;
        }
    }
}