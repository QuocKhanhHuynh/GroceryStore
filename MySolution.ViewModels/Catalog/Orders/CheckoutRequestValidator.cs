using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Orders
{
    public class CheckoutRequestValidator : AbstractValidator<CheckoutRequest>
    {
        public CheckoutRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Phải nhập tên");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Phải nhập họ");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Phải nhập email");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Phải nhập địa chỉ");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phải nhập số điện thoại");
        }
    }
}
