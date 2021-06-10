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
    public class TaskListController : BaseControllerInsertOrUpdate<TaskList, TaskListDTO, TaskListInsertOrUpdateDTO>
    {
        private readonly ITaskService _taskService;
        private readonly ITagService _tagService;

        public TaskListController(ITaskListService serviceCrud, IMapper mapper, ITaskService taskService, ITagService tagService) : base(serviceCrud, mapper)
        {
            this._taskService = taskService;
            this._tagService = tagService;
        }

        public async override Task<ActionResult<TaskListDTO>> GetById([FromRoute] Guid id)
        {
            var task = (await this._service.GetByIncludingAsync((e => e.Id == id), true, (e => e.Tasks))).FirstOrDefault();

            if (task == null)
            {
                NotFound();
            }

            var dto = this._mapper.Map<TaskListDTO>(task);
            return Ok(dto);
        }

        public override async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskListInsertOrUpdateDTO dto)
        {
            var taskList = await this._service.GetByIdAsync(id);

            if (taskList == null)
            {
                return NotFound("Not found TaskList");
            }

            var tasksIds = dto.Tasks.Where(e => e.Id != Guid.Empty).Select(e => e.Id).AsEnumerable();
            var tasks = await this._taskService.GetByIdsAsync(tasksIds);

            if (tasksIds.Count() != tasks.Count)
            {
                return NotFound("Not found Tasks.");
            }

            var tagsDto = dto.Tasks.SelectMany(e => e.Tags);
            var tagsIds = tagsDto.Where(e => e.Id != Guid.Empty).Select(e => e.Id).AsEnumerable();

            var tags = await this._tagService.GetByIdsAsync(tagsIds);

            if (tagsIds.Count() != tags.Count)
            {
                return NotFound("Not found Tags.");
            }

            this._mapper.Map(dto, taskList);

            await this._service.UpdateAsync(id, taskList);

            return Ok();
        }
    }
}
