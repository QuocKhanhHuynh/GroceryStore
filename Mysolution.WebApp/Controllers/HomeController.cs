using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySolution.ApiIntergration;
using MySolution.WebApp.Models;
using System.Diagnostics;
using System.Linq;

namespace MySolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApi;

        public HomeController(ISlideApiClient slideApiClient, IProductApiClient productApiClient, ICategoryApiClient categoryApi)
        {
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _categoryApi = categoryApi;
        }

        public async Task<IActionResult> Index()
        {
            var Categories = (await _categoryApi.GetAll()).ResultObj;
            ViewBag.Categories = Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var products = (await _productApiClient.GetProduct()).ResultObj;
            var view = new HomeControllerModel()
            {
                slides = (await _slideApiClient.GetAll()).ResultObj,
                feattured = products.Where(x => x.IsFeatured == true).ToList(),
                categories = Categories
            };
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (TempData["Order"] != null)
            {
                ViewBag.Success = TempData["Order"];
            }
            return View(view);
        }
    }
}