using MySolution.Models.Catalog.ProductImage;
using MySolution.Models.Catalog.Products;

namespace MySolution.WebApp.Models
{
    public class ProductDetailModel
    {
        public ProductViewModel product { get; set; }
        public List<ProductImageViewModel> slideImages { get; set; }
        public List<ProductViewModel> rateProduct { get; set; }
    }
}