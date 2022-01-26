using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using StartBootstrap.Data.ViewModels.Account;

namespace StartBootstrap.Validators.Account
{
    public class LoginVMValidator:AbstractValidator<LoginViewModel>
    {
        public LoginVMValidator()
        {
            RuleFor(user => user.Username).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(user => user.Password).NotNull().NotEmpty().MaximumLength(255);
        }
    }
}
