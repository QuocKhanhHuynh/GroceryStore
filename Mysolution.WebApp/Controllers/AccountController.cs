using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MySolution.ApiIntergration;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySolution.Models.System.Users;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace MySolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApi;
        public AccountController(IUserApiClient userApiClient, IConfiguration configuration, ICategoryApiClient categoryApi)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _categoryApi = categoryApi;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var token = await _userApiClient.Login(request);
            if (!token.IsSuccessed)
            {
                ModelState.AddModelError("", token.Message);
                return View(request);
            }
            var userPrincial = this.ValidateToken(token.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", token.ResultObj);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincial, authProperties);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home");
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
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Success"] = "Đăng ký thàng công";
            return RedirectToAction("Login", "account");
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _userApiClient.ForgetPassword(request);
            if (!result.IsSuccessed)
            {
                return View(request);
            }
            TempData["Success"] = "Thay đổi mật khẩu thành công";
            return RedirectToAction("login", "account");
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            var Categories = (await _categoryApi.GetAll()).ResultObj;
            ViewBag.categories = Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var id = User.Claims.First(x => x.Type == "id").ToString().Remove(0, 4);
            var user = await _userApiClient.GetById(id);
            var request = new UpdateRequest()
            {
                Id = user.ResultObj.Id,
                FirstName = user.ResultObj.FirstName,
                LastName = user.ResultObj.LastName,
                Email = user.ResultObj.Email,
                PhoneNumber = user.ResultObj.NumberPhone,
                Dob = user.ResultObj.Dob
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRequest request)
        {
            var result = await _userApiClient.Update(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            TempData["Success"] = "Cập nhật thành công";
            return RedirectToAction("update", "account");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var Categories = (await _categoryApi.GetAll()).ResultObj;
            ViewBag.categories = Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
          var result = await _userApiClient.ChangePassword(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("",result.Message);
                return View(request);
            }
            TempData["Success"] = "Cập nhật thành công";
            return RedirectToAction("update", "account");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Tokens:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Tokens:Issuer"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
            };
            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
        }
    }
}
