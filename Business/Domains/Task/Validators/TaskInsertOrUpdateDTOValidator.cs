using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class TaskInsertOrUpdateDTOValidator: AbstractValidator<TaskInsertOrUpdateDTO>, IBaseValidator
    {
        public TaskInsertOrUpdateDTOValidator(IValidator<TagInsertOrUpdateDTO> validator)
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(v => v.Priority).NotEmpty().WithMessage("Priority is required.");
            RuleFor(v => v.TaskListId).NotEqual(Guid.Empty).WithMessage("TaskListId must be informed.");
            RuleForEach(v => v.Tags).SetValidator(validator).When(e => e.Tags != null && e.Tags.Count() > 0);
        }
    }
}
