using AutoMapper;
using Business;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture
{
    [EnableCors("RestApi")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TDomain, TDTO> : ControllerBase where TDomain: BaseDomain where TDTO: class
    {
        protected Account _account => (Account)HttpContext.Items["Account"];
        protected readonly IServiceBase<TDomain> _service;
        protected readonly IMapper _mapper;

        public BaseController(IServiceBase<TDomain> service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<ICollection<TDTO>>> GetAll()
        {
            var domains = await this._service.GetAllAsync();
            var result = this._mapper.Map<ICollection<TDTO>>(domains);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDTO>> GetById([FromRoute] Guid id)
        {
            var domain = await this._service.GetByIdAsync(id);
            if (domain == null)
            {
                return NotFound();
            }

            var dto = this._mapper.Map<TDTO>(domain);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var domain = await this._service.GetByIdAsync(id);
            if (domain == null)
            {
                return NotFound();
            }

            await this._service.DeleteAsync(id);

            return Ok();
        }

    }
}
