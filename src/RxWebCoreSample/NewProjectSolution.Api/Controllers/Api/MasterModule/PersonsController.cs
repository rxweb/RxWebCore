using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewProjectSolution.Domain.MasterModule;
using NewProjectSolution.Models.Main;
using RxWeb.Core.AspNetCore;
using RxWeb.Core.Security.Authorization;

namespace NewProjectSolution.Api.Controllers.MasterModule
{
    [ApiController]
    [Route("api/[controller]")]
	
	public class PersonsController : BaseDomainController<Person,Person>

    {
        public PersonsController(IPersonDomain domain):base(domain) {}

    }
}
