using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture;
using AutoMapper;
using Business;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace RestApi
{
    public class TagsController : BaseControllerInsertOrUpdate<Tag, TagDTO, TagInsertOrUpdateDTO>
    {
        public TagsController(ITagService serviceCrud, IMapper mapper) : base(serviceCrud, mapper)
        {
        }

        public override async Task<ActionResult<TagDTO>> GetById([FromRoute] Guid id)
        {
            var tag = (await this._service.GetByIncludingAsync((e => e.Id == id), true, (e => e.TagsTasks))).FirstOrDefault();

            if(tag == null)
            {
                NotFound();
            }

            var dto = this._mapper.Map<TagDTO>(tag);
            return Ok(dto);
        }
    }
}
