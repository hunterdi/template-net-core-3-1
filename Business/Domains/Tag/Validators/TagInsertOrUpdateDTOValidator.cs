using FluentValidation;

namespace Business
{
    public class TagInsertOrUpdateDTOValidator: AbstractValidator<TagInsertOrUpdateDTO>, IBaseValidator
    {
        public TagInsertOrUpdateDTOValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
