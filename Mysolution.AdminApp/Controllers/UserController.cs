using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MySolution.ApiIntergration;
using MySolution.Models.Common;
using MySolution.Models.System.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            ViewBag.Keyword = keyword;
            var pageResult = await _userApiClient.GetUserPaging(request);
            if (TempData["result"] != null)
                ViewBag.Success = TempData["result"];
            return View(pageResult.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _userApiClient.Register(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Đăng ký thành công";
                return RedirectToAction("Index", "User");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _userApiClient.GetById(id);
            var user = new UpdateRequest()
            {
                Id = result.ResultObj.Id,
                Dob = result.ResultObj.Dob,
                Email = result.ResultObj.Email,
                FirstName = result.ResultObj.FirstName,
                LastName = result.ResultObj.LastName,
                PhoneNumber = result.ResultObj.NumberPhone
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var user = await _userApiClient.Update(request);
            if (!user.IsSuccessed)
            {
                ModelState.AddModelError("", user.Message);
                return View(request);
            }
            TempData["result"] = "Cập nhật thành công";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userApiClient.GetById(id);
            return View(user.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = new UserDeleteRequest()
            {
                Id = id
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            var result = await _userApiClient.Delete(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
            }
            TempData["result"] = "Xoá thành công";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userName)
        {
            var request = new ChangePasswordRequest()
            {
                UserName = userName
            };
            if (TempData["result"] != null)
            {
                ViewBag.Result = TempData["result"];
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _userApiClient.ChangePassword(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
            }
            TempData["result"] = "Thay đổi mật khẩu thành công";
            return RedirectToAction("ChangePassword");
        }
    }
}
