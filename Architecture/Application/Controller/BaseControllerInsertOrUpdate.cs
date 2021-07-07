using AutoMapper;
using Business;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture
{
    public abstract class BaseControllerInsertOrUpdate<TDomain, TDTO, TInsertOrUpdateDTO> : BaseController<TDomain, TDTO>
        where TDomain: BaseDomain where TDTO: class where TInsertOrUpdateDTO: class
    {
        public BaseControllerInsertOrUpdate(IServiceBase<TDomain> service, IMapper mapper) : base(service, mapper)
        {
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TInsertOrUpdateDTO dto)
        {
            var domain = this._mapper.Map<TDomain>(dto);
            await this._service.CreateAsync(domain);

            return Ok();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TInsertOrUpdateDTO dto)
        {
            var domain = await this._service.GetByIdAsync(id);
            if (domain == null)
            {
                return NotFound();
            }

            this._mapper.Map(dto, domain);

            await this._service.UpdateAsync(id, domain);

            return Ok();
        }
    }
}
