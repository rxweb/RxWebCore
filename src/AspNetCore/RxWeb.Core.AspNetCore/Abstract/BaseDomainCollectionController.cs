using Microsoft.AspNetCore.Mvc;
using RxWeb.Core.AspNetCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxWeb.Core.AspNetCore
{
    public abstract class BaseDomainCollectionController<T> : ControllerBase where T : class
    {
        protected ICoreCollectionDomain<T> Domain { get; set; }

        public BaseDomainCollectionController(ICoreCollectionDomain<T> domain)
        {
            this.Domain = domain;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]List<T> entity)
        {
            var validations = this.Domain.AddValidation(entity);
            if (validations.Count() == 0)
            {
                await this.Domain.AddAsync(entity);
                return Created($"{HttpContext.Request.Path.ToUriComponent()}/{ApplicationExtensions.GetKeyValue(entity)}", 0);
            }
            return BadRequest(validations);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody]List<T> entity)
        {
            var validations = this.Domain.UpdateValidation(entity);
            if (validations.Count() == 0)
            {
                await this.Domain.UpdateAsync(entity);
                return NoContent();
            }
            return BadRequest(validations);
        }
    }
}
