using System.Collections.Generic;
using System.Threading.Tasks;

namespace RxWeb.Core
{
    public interface ICoreDomain<T>
    {
        Task<object> GetAsync(Dictionary<string, object> parameters);
        Task<object> GetBy(Dictionary<string, object> parameters);

        HashSet<string> AddValidation(T entity);
        HashSet<string> UpdateValidation(T entity);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        HashSet<string> DeleteValidation(Dictionary<string, object> parameters);
        Task DeleteAsync(Dictionary<string, object> parameters);
    }
}
