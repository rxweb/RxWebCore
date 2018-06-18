using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Rx.AspNetCore.EntityFramework
{
    public static class Utilities
    {
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(int id)
        {
            var entityType = typeof(TEntity);
            var property = entityType.GetProperties().SingleOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0);
            var keyName = string.Empty;
            if (property == null)
                throw new NullReferenceException();
            else
                keyName = property.Name;
            var item = Expression.Parameter(entityType, "entity");
            var prop = Expression.Property(item, keyName);
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }

    }
}
