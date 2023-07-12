using MySolution.Models.Catalog.Categories;
using MySolution.Models.Catalog.Products;
using MySolution.Models.Catalog.Slides;

namespace MySolution.WebApp.Models
{
    public class HomeControllerModel
    {
        public List<SlideViewModel> slides { get; set; }
        public List<ProductViewModel> feattured { get; set; }
        public  List<CategoryViewModel> categories { get; set; }
    }
}
