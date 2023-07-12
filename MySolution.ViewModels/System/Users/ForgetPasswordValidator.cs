using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tên tài khoản là bắt buộc");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu là bắt buộc").MinimumLength(6).WithMessage("Mật khẩu từ 6 ký tự");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email là bắt buộc").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Không giống format email");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.ConfirmPassword != request.Password)
                    context.AddFailure("Mật khẩu không khớp");
            });
        }
    }
}
