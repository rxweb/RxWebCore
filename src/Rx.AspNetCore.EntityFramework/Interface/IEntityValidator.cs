using System.Collections.Generic;

namespace Rx.AspNetCore.EntityFramework
{
    public interface IEntityValidator
    {
        Dictionary<string, string> Validate<T>(T entity) where T : class;
    }
}
