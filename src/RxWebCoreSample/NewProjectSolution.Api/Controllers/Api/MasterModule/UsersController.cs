using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewProjectSolution.UnitOfWork.Main;
using NewProjectSolution.Models.Main;
using RxWeb.Core.AspNetCore;
using RxWeb.Core.Security.Authorization;

namespace NewProjectSolution.Api.Controllers.MasterModule
{
    [ApiController]
    [Route("api/[controller]")]
	
	public class UsersController : BaseController<User,vUser,User>

    {
        public UsersController(IMasterUow uow):base(uow) {}

    }
}
