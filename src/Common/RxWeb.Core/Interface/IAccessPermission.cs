using Microsoft.AspNetCore.Http;

namespace RxWeb.Core
{
    public interface IAccessPermission
    {
        bool HaveAccess(HttpContext context, object moduleId);
    }
}
