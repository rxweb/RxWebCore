using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RxWeb.Core.Data.Models;
using System.Collections.Generic;

namespace RxWeb.Core.Data.BoundedContext
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(IOptions<DatabaseConfig> databaseConfig, IHttpContextAccessor contextAccessor, ITenantDbConnectionInfo tenantDbConnection)
        {
            ConnectionStringConfig = databaseConfig.Value.ConnectionString;
            TenantDbConnection = tenantDbConnection;
            ContextAccessor = contextAccessor;
        }

		public string Name{ get; set; }

        public string GetConnection(string keyName)
        {
			this.Name = keyName;
            return GetDbConnectionString(keyName);
        }

        private string GetDbConnectionString(string keyName)
        {
            var connectionString = string.Empty;
            if (ConnectionStringConfig.ContainsKey(keyName) && !string.IsNullOrEmpty(ConnectionStringConfig[keyName]))
            {
                connectionString = ConnectionStringConfig[keyName];
            }
            else
            {
                var hostUri = GetHostUri();
                var clientConfig = TenantDbConnection.GetAsync(hostUri).Result;
                if (clientConfig != null && clientConfig.ContainsKey(keyName))
                    connectionString = clientConfig[keyName];
            }
            return connectionString;
        }

        private string GetHostUri()
        {
            return ContextAccessor.HttpContext.Request.Host.Value;
        }


        private Dictionary<string, string> ConnectionStringConfig { get; set; }

        private IHttpContextAccessor ContextAccessor { get; set; }

        private ITenantDbConnectionInfo TenantDbConnection { get; set; }

    }
}
