using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rx.AspNetCore.EntityFramework
{
    public class DbContextManager<DbContextEntity> : IDbContextManager<DbContextEntity> where DbContextEntity : DbContext
    {
        private DbContext Context { get; set; }

        private IDbContextTransaction DbContextTransaction { get; set; }

        private List<ICoreUnitOfWork> CoreUnitOfWorks { get; set; } 
        public DbContextManager(IServiceProvider serviceProvider)
        {
            Context = (DbContext)serviceProvider.GetService(typeof(DbContextEntity));
            this.CoreUnitOfWorks = new List<ICoreUnitOfWork>();
        }

        public async Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class
        {
            var setDbSet = Context.Set<TEntity>();
            return await setDbSet.FromSql(sqlQuery, parameters).ToListAsync();
        }
        public IDbContextTransaction BeginTransaction()
        {
            DbContextTransaction = Context.Database.BeginTransaction();
            return DbContextTransaction;
        }

        public IDbContextTransaction BeginTransaction(params ICoreUnitOfWork[] coreUnitOfWorks)
        {
            DbContextTransaction = Context.Database.BeginTransaction();
            foreach (var coreUnitOfWork in coreUnitOfWorks)
            {
                coreUnitOfWork.Context.Database.UseTransaction(DbContextTransaction.GetDbTransaction());
            }
            this.CoreUnitOfWorks.AddRange(coreUnitOfWorks);
            return DbContextTransaction;
        }

        public void RollbackTransaction()
        {
            Context.Database.RollbackTransaction();
            DbContextTransaction = null;
        }

        public void Commit()
        {
            if (DbContextTransaction != null) {
                this.CoreUnitOfWorks.ForEach(t => t.Commit());
                Context.Database.CommitTransaction();
            }
            
        }
        public void Dispose() {
            this.CoreUnitOfWorks.Clear();
            Context.Dispose();
        }
    }
}
