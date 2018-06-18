using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rx.AspNetCore.EntityFramework
{
    public interface IDbContextManager<DbContextEntity> where DbContextEntity : DbContext,IDisposable
    {
        IDbContextTransaction BeginTransaction();

        IDbContextTransaction BeginTransaction(params ICoreUnitOfWork[] coreUnitOfWorks);

        void RollbackTransaction();

        void Commit();

        Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;
    }
}
