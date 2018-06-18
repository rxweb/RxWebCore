using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework
{
    public interface IEntityValidator
    {
        Dictionary<string, string> Validate<T>(T entity) where T : class;
    }
}
