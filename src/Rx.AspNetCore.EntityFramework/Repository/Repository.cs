using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rx.AspNetCore.EntityFramework
{
    public class Repository<TEntity> : IRepository<TEntity> where  TEntity : class
    {
        internal IDbContext Context;
        internal DbSet<TEntity> DbSet;

        public Repository(IDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            return DbSet.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> Queryable()
        {
            return DbSet.AsNoTracking();
        }

        public IEnumerable<TEntity> AllInclude
        (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).ToList();
        }

        public IEnumerable<TEntity> FindByInclude
          (Expression<Func<TEntity, bool>> predicate,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = query.Where(predicate).ToList();
            return results;
        }

        private IQueryable<TEntity> GetAllIncluding
        (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = DbSet.AsNoTracking();

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }
        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {

            IEnumerable<TEntity> results = DbSet.AsNoTracking()
              .Where(predicate).ToList();
            return results;
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
              .Where(predicate).ToListAsync();
        }

        public TEntity FindByKey(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return DbSet.AsNoTracking().SingleOrDefault(lambda);
        }

        public async Task<TEntity> FindByKeyAsync(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(lambda);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate) {
            return DbSet.AsNoTracking().SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().SingleAsync(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().First(predicate);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstAsync(predicate);
        }

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().LastOrDefault(predicate);
        }

        public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().LastOrDefaultAsync(predicate);
        }

        public TEntity Last(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Last(predicate);
        }

        public async Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().LastAsync(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate) {
            return DbSet.AsNoTracking().Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().CountAsync(predicate);
        }

        public IEnumerable<TEntity> Get(int pageNumber, int pageSize) 
        {
            return Get(pageNumber, pageSize);
        }

        public IEnumerable<TEntity> Get(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return Get(pageNumber, pageSize, predicate);
        }

        public IEnumerable<TEntity> Get(int pageNumber,int pageSize, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) {
            return Get(pageNumber, pageSize, predicate,includeProperties);
        }

        public Task<IEnumerable<TEntity>> GetAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(Get(pageNumber, pageSize));
        }

        public Task<IEnumerable<TEntity>> GetAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Get(pageNumber, pageSize, predicate));
        }

        public Task<IEnumerable<TEntity>> GetAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(Get(pageNumber, pageSize, predicate, includeProperties));
        }

        private IEnumerable<TEntity> Get(
            int pageNumber,int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null)
        {
            var query = DbSet.AsNoTracking();
            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));
            if (filter != null)
                query = query.Where(filter);
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            return query.ToList();
        }
    }
}
