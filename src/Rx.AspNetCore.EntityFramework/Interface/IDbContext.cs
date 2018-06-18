using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Rx.AspNetCore.EntityFramework
{
    public interface IDbContext:IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();

        ChangeTracker ChangeTracker { get; }
    }
}
