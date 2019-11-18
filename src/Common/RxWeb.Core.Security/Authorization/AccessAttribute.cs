using Microsoft.AspNetCore.Authorization;

namespace RxWeb.Core.Security.Authorization
{
    public class AccessAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "ROLE_";
        public AccessAttribute(int applicationModuleId) => ApplicationModuleId = applicationModuleId;

        public int ApplicationModuleId
        {
            get
            {
                if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var applicationModuleId))
                {
                    return applicationModuleId;
                }
                return default(int);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}
