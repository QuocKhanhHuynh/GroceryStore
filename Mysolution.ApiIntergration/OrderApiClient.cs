using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Catalog.Orders;
using MySolution.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.ApiIntergration
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpContextFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderApiClient(IHttpClientFactory httpContextFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextFactory = httpContextFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<bool>> AddOrder(CheckoutRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("/api/Orders", httpContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetOrderByUserId(string userName)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.GetAsync($"/api/Orders/{userName}");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<OrderViewModel>>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int id)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.GetAsync($"/api/Orders/{id}/detail");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<OrderDetailViewModel>>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PageResult<OrderViewModel>>> GetPasingOrder(OrderPagingRequest request)
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.GetAsync($"/api/Orders?OrderId{request.OrderId}&Status={request.Status}&PageIndex={request.PageIndex}&PageSize={request.PageSize}");
            return JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<OrderViewModel>>> (await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> UpdateStatusOrder(UpdateStatusOrderRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.PatchAsync("/api/Orders", httpContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }
    }
}
