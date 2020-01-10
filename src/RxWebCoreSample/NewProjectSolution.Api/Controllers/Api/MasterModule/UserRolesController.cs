using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewProjectSolution.Domain.MasterModule;
using NewProjectSolution.Models.Main;
using RxWeb.Core.AspNetCore;
using RxWeb.Core.Security.Authorization;
using NewProjectSolution.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace NewProjectSolution.Api.Controllers.MasterModule
{

    [ApiController]
    [Route("api/users/{UserId}/[controller]")]
    [Access(1,"get")]
	public class UserRolesController : BaseDomainController<UserRole, QueryModel>

    {
        public UserRolesController(IUserRoleDomain domain):base(domain) {}

    }
}
