using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework
{
    public interface IAuditLog
    {
        void RequestLog();
        void EntityLog(object entity, EntityState entityState, object dbEntity);
        int SaveChanges();
    }
}
