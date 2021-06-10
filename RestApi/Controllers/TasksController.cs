using System;
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
    public class TasksController : BaseControllerInsertOrUpdate<Tasks, TaskDTO, TaskInsertOrUpdateDTO>
    {
        private readonly ITagService _tagService;

        public TasksController(ITaskService serviceCrud, IMapper mapper, ITagService tagService) : base(serviceCrud, mapper)
        {
            this._tagService = tagService;
        }

        public override async Task<ActionResult<TaskDTO>> GetById([FromRoute] Guid id)
        {
            var task = (await this._service.GetByIncludingAsync((e => e.Id == id), true, (e => e.TagsTasks), (e => e.TaskLists))).FirstOrDefault();

            if (task == null)
            {
                NotFound();
            }

            var dto = this._mapper.Map<TaskDTO>(task);

            return Ok(dto);
        }

        public override async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskInsertOrUpdateDTO dto)
        {
            var task = await this._service.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound("Not found Task");
            }

            var tagsIds = dto.Tags.Where(e => e.Id != Guid.Empty).Select(e => e.Id).AsEnumerable();
            var tags = await this._tagService.GetByIdsAsync(tagsIds);

            if (tagsIds.Count() != tags.Count)
            {
                return NotFound("Not found Tags.");
            }

            this._mapper.Map(dto, task);

            await this._service.UpdateAsync(id, task);

            return Ok();
        }
    }
}
