using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class TaskListInsertOrUpdateDTOValidator : AbstractValidator<TaskListInsertOrUpdateDTO>, IBaseValidator
    {
        public TaskListInsertOrUpdateDTOValidator(IValidator<TaskInsertOrUpdateDTO> taskValidator, IValidator<TagInsertOrUpdateDTO> tagValidator)
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.");
            RuleForEach(v => v.Tasks).SetValidator(taskValidator).When(e => e.Tasks != null && e.Tasks.Count() > 0 && e.Id != Guid.Empty);
            RuleForEach(v => v.Tasks).ChildRules(t =>
            {
                t.RuleFor(v => v.Title).NotEmpty().WithMessage("Title is required.");
                t.RuleFor(v => v.Priority).NotEmpty().WithMessage("Priority is required.");
                t.RuleForEach(v => v.Tags).SetValidator(tagValidator).When(e => e.Tags != null && e.Tags.Count() > 0);
            }).When(e => e.Tasks != null && e.Tasks.Count() > 0 && e.Id == Guid.Empty);
        }
    }
}
