using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên là bắt buộc");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá là bắt buộc");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Số lượng là bắt buộc");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Loại là bắt buộc");
            RuleFor(x => x.Descrestion).NotEmpty().WithMessage("Mô tả là bắt buộc");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("Seo alias là bắt buộc");
            RuleFor(x => x.Caption).NotEmpty().WithMessage("Tiêu đề là bắt buộc");
           // RuleFor(x => x.ThumbnailImage).NotEmpty().WithMessage("Ảnh sản phẩm là bắt buộc");
        }
    }
}
