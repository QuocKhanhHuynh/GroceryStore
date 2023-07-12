using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySolution.ApiIntergration;
using MySolution.Models.Catalog.Products;
using MySolution.WebApp.Models;

namespace MySolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pagedSize = 12)
        {
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var request = new GetProductPagingRequest()
            {
                keyword = keyword,
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pagedSize
            };
            var products = (await _productApiClient.GetProduct()).ResultObj;
            var productModel = new ProductIndexModel()
            {
                Products = (await _productApiClient.GetAllProduct(request)).ResultObj,
                LateProducts = products.OrderByDescending(x => x.DateCreate).Take(6).ToList(),
                DiscountProduct = products.Where(x => x.ViewCount > 0).ToList()
            };
            ViewBag.Keyword = keyword;
            return View(productModel);
        }

        public async Task<IActionResult> Detail(int productId)
        {
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var products = (await _productApiClient.GetProduct()).ResultObj;
            var product = (await _productApiClient.GetProductById(productId)).ResultObj; //x.Category.Equals(product.Category) && 
            var rateProduct = products.Where(x => x.Category.Equals(product.Category) && x.Id != productId).ToList();
            var productDetailModel = new ProductDetailModel()
            {
                product = product,
                slideImages = (await _productApiClient.GetListImage(productId)).ResultObj,
                rateProduct = rateProduct
            };
            return View(productDetailModel);
        }
    }
}
