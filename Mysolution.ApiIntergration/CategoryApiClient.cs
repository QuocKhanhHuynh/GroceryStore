using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MySolution.ApiIntergration
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respones = await client.GetAsync("api/Categories");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<CategoryViewModel>>>(await respones.Content.ReadAsStringAsync());
        }
    }
}
