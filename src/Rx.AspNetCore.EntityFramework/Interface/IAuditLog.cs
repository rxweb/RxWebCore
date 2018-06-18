using Microsoft.EntityFrameworkCore;

namespace Rx.AspNetCore.EntityFramework
{
    public interface IAuditLog
    {
        void RequestLog();
        void EntityLog(object entity, EntityState entityState, object dbEntity);
        int SaveChanges();
    }
}
