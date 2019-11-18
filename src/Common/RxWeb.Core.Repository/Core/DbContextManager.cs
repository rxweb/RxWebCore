using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RxWeb.Core.Data
{
    public class DbContextManager<DbContextEntity> : IDbContextManager<DbContextEntity>  where DbContextEntity : DbContext
    {
        private DbContext  Context { get; set; }

        private IDbContextTransaction DbContextTransaction { get; set; }
        public DbContextManager(IServiceProvider serviceProvider) {
            Context = (DbContext)serviceProvider.GetService(typeof(DbContextEntity));
        }

        public async Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class {
            var setDbSet = Context.Set<TEntity>();
           return await setDbSet.FromSql(sqlQuery, parameters).ToListAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync() {
            DbContextTransaction = await Context.Database.BeginTransactionAsync();
            return DbContextTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(params ICoreUnitOfWork[] coreUnitOfWorks)
        {
            DbContextTransaction = await Context.Database.BeginTransactionAsync();
            foreach (var coreUnitOfWork in coreUnitOfWorks) {
                coreUnitOfWork.Context.Database.UseTransaction(DbContextTransaction.GetDbTransaction());
            }
            return DbContextTransaction;
        }

        public void RollbackTransaction() {
            Context.Database.RollbackTransaction();
            DbContextTransaction = null;
        }

        public Task CommitAsync() {
            if(DbContextTransaction != null)
                Context.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public async Task CommitAsync(params ICoreUnitOfWork[] coreUnitOfWorks) {
            using (var transaction = Context.Database.BeginTransaction()) {
                foreach (var coreUnitOfWork in coreUnitOfWorks) {
                    coreUnitOfWork.Context.Database.UseTransaction(transaction.GetDbTransaction());
                    await coreUnitOfWork.CommitAsync();
                }
                transaction.Commit();
            }
        }
    }
}
