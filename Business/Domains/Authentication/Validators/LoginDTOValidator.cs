using Business;
using FluentValidation;

namespace Business
{
    public class LoginDTOValidator: AbstractValidator<LoginDTO>, IBaseValidator
    {
        public LoginDTOValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Email invalid.");
            RuleFor(v => v.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
