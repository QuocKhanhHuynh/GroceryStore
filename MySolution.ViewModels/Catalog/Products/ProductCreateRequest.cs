using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Giá")]
        public int Price { get; set; }
        [Display(Name = "Số lượng")]
        public int Stock { get; set; }
        [Display(Name = "Giảm giá")]
        public int ViewCount { get; set; }
        [Display(Name = "Nhóm thực phẩm")]
        public int CategoryId { get; set; }
        [Display(Name = "Mô tả")]
        public string Descrestion { get; set; }
        public string SeoAlias { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int SortOrder { get; set; }
        

        [Display(Name = "Sản phẩm nổi bật")]
        public bool IsFeatured { get; set; }
        [Display(Name = "Ảnh 1 (ảnh đại diện)")]
        public IFormFile? ThumbnailImage1 { get; set; }
        [Display(Name = "Ảnh 2")]
        public IFormFile? ThumbnailImage2 { get; set; }
        [Display(Name = "Ảnh 3")]
        public IFormFile? ThumbnailImage3 { get; set; }
        [Display(Name = "Ảnh 4")]
        public IFormFile? ThumbnailImage4 { get; set; }
        [Display(Name = "Ảnh 5")]
        public IFormFile? ThumbnailImage5 { get; set; }
    }
}
