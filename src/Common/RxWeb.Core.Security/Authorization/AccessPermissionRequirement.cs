using Microsoft.AspNetCore.Authorization;

namespace RxWeb.Core.Security.Authorization
{
    public class AccessPermissionRequirement : IAuthorizationRequirement
    {
        public int ApplicationModuleId { get; private set; }

        public AccessPermissionRequirement(int applicationModuleId) { ApplicationModuleId = applicationModuleId; }
    }
}
