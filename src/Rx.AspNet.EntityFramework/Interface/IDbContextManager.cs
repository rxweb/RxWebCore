using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework
{
    public interface IDbContextManager<DbContextEntity> where DbContextEntity : DbContext,IDisposable
    {
        DbContextTransaction BeginTransaction();

        DbContextTransaction BeginTransaction(params ICoreUnitOfWork[] coreUnitOfWorks);

        void RollbackTransaction();

        void Commit();

        Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;
    }
}
