using MySolution.Models.Catalog.Products;
using MySolution.Models.Common;

namespace MySolution.WebApp.Models
{
    public class ProductIndexModel
    {
        public PageResult<ProductViewModel> Products { get; set; }
        public List<ProductViewModel> LateProducts { get; set; }
        public List<ProductViewModel> DiscountProduct { get; set; } = new List<ProductViewModel>();
    }
}
