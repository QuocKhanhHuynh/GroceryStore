using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySolution.ApiIntergration;
using MySolution.Models.Catalog.Products;

namespace MySolution.AdminApp.Controllers
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
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            ViewBag.keyword = keyword;
            var Categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && x.Id == categoryId.Value
            });
            if (TempData["result"] != null)
            {
                ViewBag.success = TempData["result"];
            }
            var PVM = await _productApiClient.GetAllProduct(request);
            return View(PVM.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = (await _productApiClient.Create(request)).ResultObj;
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Không thể tạo tài khoản");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = (await _productApiClient.GetProductById(id)).ResultObj;
            var productEdit = new ProductUpdateRequest()
            {
                Id = product.Id,
                Descrestion = product.Descrestion,
                IsFeatured = product.IsFeatured,
                Price = product.Price,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return View(productEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = (await _productApiClient.Update(request)).ResultObj;
            if (result)
            {
                TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Không thể cập nhật sản phẩm");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var productId = new ProductDeleteRequest()
            {
                Id = id
            };
            return View(productId);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _productApiClient.Delete(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("",result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = (await _productApiClient.GetProductById(id)).ResultObj;
            return View(product);
        }
    }
}