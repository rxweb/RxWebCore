using Rx.AspNet.EntityFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework
{
    public interface ICoreUnitOfWork
    {
        IDbContext Context { get; set; }

        void RegisterNew<T>([NotNull] T entity) where T : class;

        void RegisterNew<T>([NotNull] IEnumerable<T> entities) where T : class;

        Task RegisterNewAsync<T>([NotNull] T entity) where T : class;

        Task RegisterNewAsync<T>([NotNull] IEnumerable<T> entities) where T : class;

        void RegisterDirty<T>([NotNull] T entity) where T : class;

        void RegisterDirty<T>([NotNull] IEnumerable<T> entities) where T : class;

        Task RegisterDirtyAsync<T>([NotNull] IEnumerable<T> entities) where T : class;

        Task RegisterDirtyAsync<T>([NotNull] T entity) where T : class;

        Task RegisterDirtyAsync<T>([NotNull] Dictionary<string, dynamic> propValues, int id) where T : class;

        void RegisterDirty<T>([NotNull] Dictionary<string, dynamic> propValues, int id) where T : class;

        void RegisterClean<T>([NotNull] T entity) where T : class;

        void RegisterClean<T>([NotNull] IEnumerable<T> entities) where T : class;

        Task RegisterCleanAsync<T>([NotNull] IEnumerable<T> entities) where T : class;

        Task RegisterCleanAsync<T>([NotNull] T entity) where T : class;

        void RegisterDeleted<T>([NotNull] T entity) where T : class;

        void RegisterDeleted<T>([NotNull] IEnumerable<T> entities) where T : class;

        void RegisterDeletedAsync<T>([NotNull] IEnumerable<T> entities) where T : class;

        void RegisterDeletedAsync<T>([NotNull] T entity) where T : class;

        void RegisterDeleted<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task RegisterDeletedAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        void RegisterDeleted<T>(int id) where T : class;

        Task RegisterDeletedAsync<T>(int id) where T : class;

        int Commit();
        Task<int> CommitAsync();
        void Refresh();

        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        IQueryExecutor Query { get; set; }

    }
}
