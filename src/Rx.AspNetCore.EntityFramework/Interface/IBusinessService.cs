namespace Rx.AspNetCore.EntityFramework
{
    public interface IBusinessService
    {
        object Process<T>(T entity) where T : class;
    }
}
