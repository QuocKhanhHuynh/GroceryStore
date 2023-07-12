using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mysolution.AdminApp.Models;
using MySolution.ApiIntergration;
using MySolution.Models.Catalog.Orders;
using MySolution.Models.Common;
using System.Diagnostics;

namespace MySolution.AdminApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IOrderApiClient _orderApiClient;
        public HomeController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? orderId, int? status, int pageIndex = 1, int pageSize = 10)
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            if (orderId != null)
            {
                ViewBag.OrderId = orderId;
            }
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Đang xử lí", Value = "0", Selected = status.HasValue&&status==0? true : false },
                new SelectListItem() {Text = "Đã xác nhận", Value = "1", Selected = status.HasValue&&status==1? true : false },
                new SelectListItem() {Text = "Đang vận chuyển", Value = "2", Selected = status.HasValue&&status==2? true : false },
                new SelectListItem() {Text = "Đã nhận hàng", Value = "3", Selected = status.HasValue&&status==3? true : false }
            };
            ViewBag.Status = selectList;
            var request = new OrderPagingRequest()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Status = status,
                OrderId = orderId
            };
            var orders = await _orderApiClient.GetPasingOrder(request);
            return View(orders.ResultObj);
        }

        public async Task<IActionResult> Details(int id, int status)
        {
            var order = await _orderApiClient.GetOrderDetail(id);
            ViewBag.Status = status;
            ViewBag.Id = id;
            return View(order.ResultObj);
        }

        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var request = new UpdateStatusOrderRequest()
            {
                Id = id,
                Status = status
            };
            var result = await _orderApiClient.UpdateStatusOrder(request);
            if (result.IsSuccessed)
            {
                TempData["Success"] = "Cập nhật thành công";
            }
            return RedirectToAction("index", "Home");
        }
    }
}