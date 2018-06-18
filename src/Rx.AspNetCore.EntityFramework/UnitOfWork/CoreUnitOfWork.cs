using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rx.AspNetCore.EntityFramework.Attributes;
using Rx.AspNetCore.EntityFramework.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rx.AspNetCore.EntityFramework
{
    public abstract class CoreUnitOfWork : ICoreUnitOfWork, IDisposable
    {

        public IDbContext Context { get; set; }

        public IRepositoryProvider RepositoryProvider { get; set; }

        public IQueryExecutor Query { get; set; }

        private IAuditLog AuditLog { get; set; }

        private List<object> AddedEntities { get; set; } = new List<object>();

        private Type InstanceType { get; set; }

        private bool IsAuditable { get; set; }

        public virtual void RegisterNew<T>([NotNull] T entity) where T : class
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
                dbEntityEntry.State = EntityState.Added;
            else
            {
                var dbSet = Context.Set<T>();
                dbSet.Add(entity);
            }
        }

        public virtual void RegisterNew<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
                RegisterNew(entity);
        }

        public virtual Task RegisterNewAsync<T>([NotNull] T entity) where T : class
        {
            RegisterNew<T>(entity);
            return Task.FromResult(0);
        }

        public virtual Task RegisterNewAsync<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            RegisterNew<T>(entities);
            return Task.FromResult(0);
        }

        public virtual void RegisterDirty<T>([NotNull] Dictionary<string, dynamic> propValues, int id) where T : class {
            var entity = this.Repository<T>().FindByKey(id);
            if (entity != null) {
                var properties = entity.GetType().GetProperties();
                var propEnumerator = propValues.GetEnumerator();
                while (propEnumerator.MoveNext()) {
                    var current = propEnumerator.Current;
                    var property = properties.Single(t => t.Name == current.Key);
                    property.SetValue(entity, current.Value);
                }
                RegisterDirty(entity);
            }
        }

        public virtual Task RegisterDirtyAsync<T>([NotNull] Dictionary<string, dynamic> propValues, int id) where T : class
        {
            RegisterDirty<T>(propValues, id);
            return Task.FromResult(0);
        }
        public virtual void RegisterDirty<T>([NotNull] T entity) where T : class
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                var dbSet = Context.Set<T>();
                dbSet.Attach(entity);
            }
            SetEntryState(entity, EntityState.Modified);
        }

        public virtual void RegisterDirty<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            var dbSet = Context.Set<T>();
            dbSet.AttachRange(entities);
        }

        public virtual Task RegisterDirtyAsync<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            RegisterDirty(entities);
            return Task.FromResult(0);
        }

        public virtual Task RegisterDirtyAsync<T>([NotNull] T entity) where T : class
        {
            RegisterDirty(entity);
            return Task.FromResult(0);
        }

        public virtual void RegisterClean<T>([NotNull] T entity) where T : class =>
            SetEntryState(entity, EntityState.Unchanged);

        public virtual void RegisterClean<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
                RegisterClean(entity);
        }

        public virtual Task RegisterCleanAsync<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            RegisterClean<T>(entities);
            return Task.FromResult(0);
        }

        public virtual Task RegisterCleanAsync<T>([NotNull] T entity) where T : class
        {
            RegisterClean<T>(entity);
            return Task.FromResult(0);
        }

        public virtual void RegisterDeleted<T>([NotNull] T entity) where T : class
        {
            var dbSet = Context.Set<T>();
            dbSet.Remove(entity);
            SetEntryState(entity, EntityState.Deleted);
        }

        public virtual void RegisterDeleted<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            var dbSet = Context.Set<T>();
            dbSet.RemoveRange(entities);
        }

        public virtual void RegisterDeletedAsync<T>([NotNull] IEnumerable<T> entities) where T : class
        {
            RegisterDeleted(entities);
            Task.FromResult(0);
        }

        public virtual void RegisterDeletedAsync<T>([NotNull] T entity) where T : class
        {
            RegisterDeleted(entity);
            Task.FromResult(0);
        }


        public virtual void RegisterDeleted<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var entities = this.Repository<T>().FindBy(predicate);
            RegisterDeleted(entities);
        }

        public virtual Task RegisterDeletedAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var entities = this.Repository<T>().FindBy(predicate);
            RegisterDeleted(entities);
            return Task.FromResult(0);
        }

        public virtual void RegisterDeleted<T>(int id) where T : class
        {
            var entity = this.Repository<T>().FindByKey(id);
            RegisterDeleted(entity);
        }

        public virtual Task RegisterDeletedAsync<T>(int id) where T : class
        {
            var entity = this.Repository<T>().FindByKey(id);
            RegisterDeleted(entity);
            return Task.FromResult(0);
        }
        

        public virtual int Commit()
        {
            SetAudit();
            var id = Context.SaveChanges();
            if (this.AuditLog != null)
            {
                this.AddedEntities.ForEach(t =>
                {
                    this.AuditLog.EntityLog(t, EntityState.Added, null);
                });
                this.AuditLog.SaveChanges();
            }
            return id;
        }

        public virtual Task<int> CommitAsync()
        {
            var id = Commit();
            return Task.FromResult(id);
        }

        private void SetEntryState<T>(T entity, EntityState state) where T : class
        {
            Context.Entry(entity).State = state;
        }

        private void SetAudit()
        {
            if (this.AuditLog != null)
            {
                this.AuditLog.RequestLog();
                Context.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified || t.State == EntityState.Added || t.State == EntityState.Deleted).ToList().ForEach(entity =>
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            this.AddedEntities.Add(entity.Entity);
                            break;
                        default:
                            this.LogAudit(entity);
                            break;
                    }
                });
            }

        }

        private void LogAudit(EntityEntry entityEntry)
        {
            var entity = entityEntry.Entity;
            var state = entityEntry.State;
            var entityType = entity.GetType();
            var recordLog = entityType.GetCustomAttributes(typeof(RecordLogAttribute), false).SingleOrDefault() as RecordLogAttribute;
            if (recordLog != null)
            {
                var keyValue = entityType.GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).GetValue(entity);
                var dbEntity = this.RepositoryProvider.GetRepositoryByType(entityType, InstanceType).FindByKey(Convert.ToInt32(keyValue));
                this.AuditLog.EntityLog(entity, state, dbEntity);
            }
        }
        public void Refresh()
        {
            Context.ChangeTracker.Entries().ToList().ForEach(x =>
             {
                 x.State = EntityState.Detached;
             });
        }

        public CoreUnitOfWork(IDbContext context, IRepositoryProvider repositoryProvider)
        {
            this.SetContextRepository(context, repositoryProvider);
        }

        public CoreUnitOfWork(IDbContext context, IRepositoryProvider repositoryProvider, IAuditLog auditLog)
        {
            this.AuditLog = auditLog;
            this.SetContextRepository(context, repositoryProvider);
        }
        private void SetContextRepository(IDbContext context, IRepositoryProvider repositoryProvider)
        {
            InstanceType = context.GetType();
            Context = (IDbContext)context;
            RepositoryProvider = repositoryProvider;
            Query = new QueryExecutor(Context);
            DataUtility.DbConnectionString = Context.Database.GetDbConnection().ConnectionString;
        }


        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return RepositoryProvider.Repository<TEntity>(Context, InstanceType);
        }

        public void Dispose()
        {
            InstanceType = null;
            this.AddedEntities.Clear();
            Context.Dispose();
            RepositoryProvider.Dispose();
        }
    }
}
