using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data.Entity;

namespace Rx.AspNet.EntityFramework
{
    public class DbContextManager<DbContextEntity> : IDbContextManager<DbContextEntity> where DbContextEntity : DbContext
    {
        private DbContext Context { get; set; }

        private DbContextTransaction DbContextTransaction { get; set; }

        private List<ICoreUnitOfWork> CoreUnitOfWorks { get; set; } 
        public DbContextManager(IServiceProvider serviceProvider)
        {
            Context = (DbContext)serviceProvider.GetService(typeof(DbContextEntity));
            this.CoreUnitOfWorks = new List<ICoreUnitOfWork>();
        }

        public async Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class
        {
            return await Context.Database.SqlQuery<TEntity>(sqlQuery, parameters).ToListAsync();
        }
        public DbContextTransaction BeginTransaction()
        {
            DbContextTransaction = Context.Database.BeginTransaction();
            return DbContextTransaction;
        }

        public DbContextTransaction BeginTransaction(params ICoreUnitOfWork[] coreUnitOfWorks)
        {
            DbContextTransaction = Context.Database.BeginTransaction();
            foreach (var coreUnitOfWork in coreUnitOfWorks)
            {
                coreUnitOfWork.Context.Database.UseTransaction(DbContextTransaction.UnderlyingTransaction);
            }
            this.CoreUnitOfWorks.AddRange(coreUnitOfWorks);
            return DbContextTransaction;
        }

        public void RollbackTransaction()
        {
            DbContextTransaction.Rollback();
            DbContextTransaction = null;
        }

        public void Commit()
        {
            if (DbContextTransaction != null) {
                this.CoreUnitOfWorks.ForEach(t => t.Commit());
                DbContextTransaction.Commit();
            }
            
        }
        public void Dispose() {
            this.CoreUnitOfWorks.Clear();
            Context.Dispose();
        }
    }
}
