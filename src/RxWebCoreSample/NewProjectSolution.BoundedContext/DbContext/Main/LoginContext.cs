using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewProjectSolution.BoundedContext.SqlContext;
using NewProjectSolution.Models.Main;
using NewProjectSolution.Models;
using NewProjectSolution.BoundedContext.Singleton;
using RxWeb.Core.Data;

namespace NewProjectSolution.BoundedContext.Main
{
    public class LoginContext : BaseBoundedDbContext, ILoginContext
    {
        public LoginContext(MainSqlDbContext sqlDbContext,  IOptions<DatabaseConfig> databaseConfig, IHttpContextAccessor contextAccessor,TenantDbConnectionInfo tenantDbConnection): base(sqlDbContext, databaseConfig.Value, contextAccessor,tenantDbConnection){ }

            #region DbSets
            		public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<vUser> Users { get; set; }

        
        #endregion DbSets


    }


    public interface ILoginContext : IDbContext
    {
    }
}

