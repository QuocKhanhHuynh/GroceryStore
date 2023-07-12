using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySolution.BackendApi.Data.Entities;
using MySolution.Models.Catalog.Slides;
using MySolution.Models.Common;
using MySolution.Models.System.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userMangager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        public UsersController(UserManager<User> userMangager, SignInManager<User> signInManager, IConfiguration config)
        {
            _userMangager = userMangager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userMangager.FindByNameAsync(request.UserName);
            if (user == null)
                return BadRequest(new ApiErrorResult<string>("Tài khoản không tồn tại"));
            if (request.IsAdmin != null && request.IsAdmin == true && user.IsAdmin == false)
            {
                return BadRequest(new ApiErrorResult<string>("Tài khoản không đượcn phép truy cập"));
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return BadRequest(new ApiErrorResult<string>("Mật khẩu đăng nhập sai"));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("id",user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    _config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds
                );
            return Ok(new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token)));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userMangager.FindByNameAsync(request.UserName) != null)
                return BadRequest(new ApiErrorResult<bool>("Tên tài khoản đã tồn tại"));
            if (await _userMangager.FindByEmailAsync(request.Email) != null)
                return BadRequest(new ApiErrorResult<bool>("Email đã tồn tại"));
            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsAdmin = (request.IsAdmin != null && request.IsAdmin == true) ? true : false,
            };
            var result = await _userMangager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return Ok(new ApiSuccessResult<bool>());
            return BadRequest(new ApiErrorResult<bool>("Không thể tạo tài khoản"));
        }

        [HttpGet("paging")]
        [Authorize]
        public async Task<IActionResult> GetUserPaging([FromQuery]GetUserPagingRequest request)
        {
            var query = _userMangager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new UserViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                NumberPhone = x.PhoneNumber
            }).ToListAsync();
            var pageResult = new PageResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return Ok(new ApiSuccessResult<PageResult<UserViewModel>>(pageResult));
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userMangager.FindByIdAsync(id);
            if (user == null)
                return BadRequest(new ApiErrorResult<UserViewModel>("Tài khoản không tồn tại"));
            var getUser = new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NumberPhone = user.PhoneNumber,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin,
            };
            return Ok(new ApiSuccessResult<UserViewModel>(getUser));
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            if (await _userMangager.Users.AnyAsync(x => x.Email.Contains(request.Email) && x.Id != request.Id))
                return BadRequest(new ApiErrorResult<bool>("Email đã tồn tại"));
            var user = await _userMangager.FindByIdAsync(request.Id);
            if (user == null)
                return BadRequest(new ApiErrorResult<bool>("Không tìm thấy tài khoản"));
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Dob = request.Dob;
            await _userMangager.UpdateAsync(user);
            return Ok(new ApiSuccessResult<bool>());
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userMangager.FindByIdAsync(id);
            if (user == null)
                return BadRequest(new ApiErrorResult<bool>("Tài khoản không tồn tại"));
            var result = await _userMangager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiErrorResult<bool>("Không thể xóa"));
            return Ok(new ApiSuccessResult<bool>());
        }

        [HttpPatch]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var user = await _userMangager.FindByNameAsync(request.UserName);
            if (user == null)
                return BadRequest(new ApiErrorResult<bool>("Tài khoản không tồn tại"));
            if (!user.Email.Equals(request.Email))
                return BadRequest(new ApiErrorResult<bool>("Email không chính xác"));
            var hashPassword = _userMangager.PasswordHasher.HashPassword(user, request.Password);
            user.PasswordHash = hashPassword;
            var result = await _userMangager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new ApiSuccessResult<bool>());
            return BadRequest(new ApiErrorResult<bool>("Đổi mật khẩu thất bại"));
        }

        [HttpPatch("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userMangager.FindByNameAsync(request.UserName);
            if (user == null)
                return BadRequest(new ApiErrorResult<bool>("Tài khoản không tồn tại"));
            var checkPass = await _userMangager.CheckPasswordAsync(user, request.CurrentPassword);
            if (!checkPass)
            {
                return BadRequest(new ApiErrorResult<bool>("Mật khẩu hiện tại nhập vào không khớp mật khẩu hiện tại được lưu"));
            }
            var result = await _userMangager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
                return Ok(new ApiSuccessResult<bool>());
            return BadRequest(new ApiErrorResult<bool>("Đổi mật khẩu thất bại"));
        }
    }
}
