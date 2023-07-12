using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("user name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("password is required").MinimumLength(6).WithMessage("password from 6 character");
        }
    }
}
