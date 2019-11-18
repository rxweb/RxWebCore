using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewProjectSolution.BoundedContext.Singleton;
using NewProjectSolution.Models;
using NewProjectSolution.Models.Const;
using NewProjectSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace NewProjectSolution.BoundedContext.SqlContext
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(IOptions<DatabaseConfig> databaseConfig, IHttpContextAccessor contextAccessor, TenantDbConnectionInfo tenantDbConnection)
        {
            ConnectionStringConfig = databaseConfig.Value.ConnectionString;
            TenantDbConnection = tenantDbConnection;
            ContextAccessor = contextAccessor;
        }

		public DbSet<StoreProcResult> StoreProcResults { get; set; }

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

        private TenantDbConnectionInfo TenantDbConnection { get; set; }

    }
}
