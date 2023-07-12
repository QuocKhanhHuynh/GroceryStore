using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySolution.Models.Common;
using MySolution.Models.System.Users;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MySolution.ApiIntergration
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpContextFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpContextFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextFactory = httpContextFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> Delete(UserDeleteRequest request)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.DeleteAsync($"/api/Users/{request.Id}");
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> ForgetPassword(ForgetPasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var responese = await client.PatchAsync($"/api/Users", httpContent);
            if (responese.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await responese.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await responese.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<UserViewModel>> GetById(string Id)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.GetAsync($"/api/Users/{Id}");
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserViewModel>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<UserViewModel>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.GetAsync($"/api/Users/paging?Keyword={request.Keyword}&PageIndex={request.PageIndex}&PageSize={request.PageSize}");
            var users = JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<UserViewModel>>>(await response.Content.ReadAsStringAsync());
            return users;
        }

        public async Task<ApiResult<string>> Login(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/Users/Authenticate", httpContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/Users", httpContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }
        public async Task<ApiResult<bool>> Update(UpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("/api/Users", httpContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.PatchAsync("/api/Users/ChangePassword", httpContent);
            if (responese.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await responese.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await responese.Content.ReadAsStringAsync());
        }
    }
}