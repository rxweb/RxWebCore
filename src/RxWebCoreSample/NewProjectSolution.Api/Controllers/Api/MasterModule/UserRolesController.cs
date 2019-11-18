using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewProjectSolution.Domain.MasterModule;
using NewProjectSolution.Models.Main;
using RxWeb.Core.AspNetCore;
using RxWeb.Core.Security.Authorization;

namespace NewProjectSolution.Api.Controllers.MasterModule
{
    [ApiController]
    [Route("api/users/{UserId}/[controller]")]
	
	public class UserRolesController : BaseDomainController<UserRole>

    {
        public UserRolesController(IUserRoleDomain domain):base(domain) {}

    }
}
