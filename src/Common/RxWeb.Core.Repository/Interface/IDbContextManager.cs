using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RxWeb.Core.Data
{
    public interface IDbContextManager<DbContextEntity> where DbContextEntity : DbContext
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<IDbContextTransaction> BeginTransactionAsync(params ICoreUnitOfWork[] coreUnitOfWorks);

        void RollbackTransaction();

        Task CommitAsync();

        Task CommitAsync(params ICoreUnitOfWork[] coreUnitOfWorks);

        Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;
    }
}
