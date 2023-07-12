using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName là bắt buộc");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Nhập mật khẩu hiện tại");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Nhập mật khẩu mới").MinimumLength(6).WithMessage("Mật khẩu từ 6 ký tự");
        }
    }
}
