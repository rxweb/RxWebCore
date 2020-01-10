using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RxWeb.Core.AspNetCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxWeb.Core.AspNetCore
{
    public abstract class BaseDomainCollectionController<T, FromQuery> : ControllerBase where T : class where FromQuery : class
    {
        protected ICoreCollectionDomain<T, FromQuery> Domain { get; set; }

        public BaseDomainCollectionController(ICoreCollectionDomain<T, FromQuery> domain)
        {
            this.Domain = domain;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
