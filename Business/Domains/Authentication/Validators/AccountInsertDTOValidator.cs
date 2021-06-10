using Business;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class AccountInsertDTOValidator: AbstractValidator<AccountInsertDTO>, IBaseValidator
    {
        public AccountInsertDTOValidator()
        {
            RuleFor(v => v.ConfirmPassword).Equal(e => e.Password).WithMessage("The passwords is difference.");
            RuleFor(v => v.Password).NotEmpty().WithMessage("The password is required.");
            RuleFor(v => v.ConfirmPassword).NotEmpty().WithMessage("The password is required.");
            RuleFor(v => v.Email).EmailAddress().WithMessage("Email invalid.");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(v => v.FirstName).NotEmpty().WithMessage("Firstname is required.");
            RuleFor(v => v.LastName).NotEmpty().WithMessage("LastName is required.");
            RuleFor(v => v.Role).NotEmpty().WithMessage("Rola is required.");
            RuleFor(v => v.Title).NotEmpty().WithMessage("Title is required.");
        }
    }
}
