using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Products
{
    public class ProductViewModel
    {
        [Display(Name="Mã sản phẩm")]
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "Giá")]
        public int Price { get; set; }
        [Display(Name = "Số lượng")]
        public int Stock { get; set; }
        [Display(Name = "Tỷ lệ giảm giá")]
        public int ViewCount { get; set; }
        [Display(Name = "Mô tả")]
        public string Descrestion { get; set; }
        [Display(Name = "Nhóm")]
        public string Category { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string ThumbnailImage { get; set; }
        [Display(Name = "Sản phẩm đặc biệt")]
        public bool IsFeatured { get; set; }
        [Display(Name = "Ngày nhập")]
        public DateTime DateCreate { get; set; }
    }
}
