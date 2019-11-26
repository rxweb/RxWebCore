using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewProjectSolution.UnitOfWork.Main;
using NewProjectSolution.Models.Main;
using RxWeb.Core.AspNetCore;
using RxWeb.Core.Security.Authorization;
using RxWeb.Core.Cache;

namespace NewProjectSolution.Api.Controllers.MasterModule
{
    [ApiController]
    [Route("api/[controller]")]
    [CacheETag(CacheMinutes =5)]
	
	public class PersonsController : BaseController<Person,Person,Person>

    {
        public PersonsController(IMasterUow uow):base(uow) {}

    }
}
