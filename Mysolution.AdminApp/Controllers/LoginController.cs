using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using MySolution.ApiIntergration;
using MySolution.Models.System.Users;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MySolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
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
        public async Task<IActionResult> ForgetPassword()
        {
            var request = new ForgetPasswordRequest();
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            var result = await _userApiClient.ForgetPassword(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("",result.Message);
                return View(result);
            }
            TempData["Success"] = "Thay đổi mật khẩu thành công";
            return RedirectToAction("Index", "Login");
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