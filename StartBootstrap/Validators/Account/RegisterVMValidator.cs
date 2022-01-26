using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using StartBootstrap.Data.ViewModels.Account;

namespace StartBootstrap.Validators.Account
{
    public class RegisterVMValidator:AbstractValidator<RegisterViewModel>
    {
        public RegisterVMValidator()
        {
            RuleFor(user => user.FullName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(user=>user.Username).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(user=>user.Email).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(user=>user.Password).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(user=>user.PasswordConfirm).NotNull().NotEmpty().MaximumLength(255);
        }
    }
}
