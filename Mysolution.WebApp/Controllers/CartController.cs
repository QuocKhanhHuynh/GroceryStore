using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySolution.ApiIntergration;
using MySolution.Models.Catalog.Orders;
using MySolution.Models.Catalog.Products;
using MySolution.WebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MySolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IUserApiClient _userApiClient;
        public CartController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IOrderApiClient orderApiClient, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
        }

        public async  Task<IActionResult> Index()
        {
            if (TempData["insufficient"] != null)
            {
                ViewBag.insufficient = TempData["insufficient"];
            }
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        public async Task<IActionResult> Favorite()
        {
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            var favoriteSession = HttpContext.Session.GetString("Favorite");
            var favoriteItems = new List<FavoriteItemViewModel>();
            if (favoriteSession != null)
            {
                favoriteItems = JsonConvert.DeserializeObject<List<FavoriteItemViewModel>>(favoriteSession);
            }
            if (favoriteItems.Any( x=> x.ProductId == productId))
            {
                var sp = favoriteItems.First(x=> x.ProductId == productId);
                favoriteItems.Remove(sp);
                HttpContext.Session.SetString("Favorite", JsonConvert.SerializeObject(favoriteItems));
            }

            var product = (await _productApiClient.GetProductById(productId)).ResultObj;
            var session = HttpContext.Session.GetString("Cart");
            var CartItems = new List<CartItemViewModel>();
            if (session != null)
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            int quanlity = 1;
            if (CartItems.Any(x => x.ProductId == productId))
            {
                var pd = CartItems.First(x => x.ProductId == productId);
                quanlity += pd.Quanlity;
                CartItems.Remove(pd);
            }
            var item = new CartItemViewModel()
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quanlity = quanlity,
                Image = product.ThumbnailImage
            };
            CartItems.Add(item);
            
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(CartItems));
            return Ok(CartItems);
        }

        public async Task<IActionResult> AddToFavorite(int productId)
        {
            var product = (await _productApiClient.GetProductById(productId)).ResultObj;
            var session = HttpContext.Session.GetString("Favorite");
            var favoriteItems = new List<FavoriteItemViewModel>();
            if (session != null)
                favoriteItems = JsonConvert.DeserializeObject<List<FavoriteItemViewModel>>(session);
            if (!favoriteItems.Any(x => x.ProductId == productId))
            {
                var item = new FavoriteItemViewModel()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.ThumbnailImage
                };
                favoriteItems.Add(item);
            }
            HttpContext.Session.SetString("Favorite", JsonConvert.SerializeObject(favoriteItems));
            return Ok(favoriteItems);
        }

        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString("Cart");
            var CartItems = new List<CartItemViewModel>();
            if (session != null)
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(CartItems);
        }

        [HttpGet]
        public IActionResult GetListFavoriteItems()
        {
            var sessionFavorite = HttpContext.Session.GetString("Favorite");
            var FavoriteItems = new List<FavoriteItemViewModel>();
            if (sessionFavorite != null)
                FavoriteItems = JsonConvert.DeserializeObject<List<FavoriteItemViewModel>>(sessionFavorite);
			return Ok(FavoriteItems);
        }

        public IActionResult UpdateCart(int productId, int quanlity)
        {
            var session = HttpContext.Session.GetString("Cart");
            var CartItems = new List<CartItemViewModel>();
            if (session != null)
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            foreach (var item in CartItems)
            {
                if (item.ProductId == productId)
                {
                    if (quanlity == 0)
                    {
                        CartItems.Remove(item);
                        break;
                    }
                    item.Quanlity = quanlity;
                    break;
                }
            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(CartItems));
            return Ok(CartItems);
        }

        public IActionResult RemoveFavorite(int productId)
        {
            var session = HttpContext.Session.GetString("Favorite");
            var favoriteItems = new List<FavoriteItemViewModel>();
            if (session != null)
                favoriteItems = JsonConvert.DeserializeObject<List<FavoriteItemViewModel>>(session);
            var item = favoriteItems.First(x => x.ProductId == productId);
            favoriteItems.Remove(item);
            HttpContext.Session.SetString("Favorite", JsonConvert.SerializeObject(favoriteItems));
            return Ok(favoriteItems);
        }

        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var session = HttpContext.Session.GetString("Cart");
            var CartItems = new List<CartItemViewModel>();
            if (session != null)
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            foreach (var item in CartItems)
            {
                var product = (await _productApiClient.GetProductById(item.ProductId)).ResultObj;
                if (product.Stock < item.Quanlity)
                {
                    TempData["insufficient"] = $"Sản phẩm {item.Name} hiện chỉ còn {product.Stock} sản phẩm. Vui lòng giảm số lượng!";
                    return RedirectToAction("Index", "Cart");
                }
            }
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order(CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                var categories = (await _categoryApiClient.GetAll()).ResultObj;
                ViewBag.Categories = categories.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(request);
            }
            var session = HttpContext.Session.GetString("Cart");
            var CartItems = new List<CartItemViewModel>();
            if (session != null)
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            foreach (var item in CartItems)
            {
                request.Details.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quanlity = item.Quanlity
                });
            }
            var result = await _orderApiClient.AddOrder(request);
            if (!result.IsSuccessed)
            {
                return View(request);
            }
            TempData["Order"] = "Đã thanh toán";
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("GetOrder", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            if (TempData["Order"] != null)
            {
                ViewBag.Success = TempData["Order"];
            }
            var userName = User.Identity.Name;
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var orders = (await _orderApiClient.GetOrderByUserId(userName)).ResultObj;
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetail(int id, int status)
        {
            ViewBag.Status = status;
            var categories = (await _categoryApiClient.GetAll()).ResultObj;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var orders = (await _orderApiClient.GetOrderDetail(id)).ResultObj;
            return View(orders);
        }
    }
}
