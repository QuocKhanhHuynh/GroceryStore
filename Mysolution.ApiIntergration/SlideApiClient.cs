using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySolution.Models.Catalog.Slides;
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
    public class SlideApiClient : ISlideApiClient
    {
        private readonly IHttpClientFactory _httpContextFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SlideApiClient(IHttpClientFactory httpContextFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextFactory = httpContextFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<List<SlideViewModel>>> GetAll()
        {
            var client = _httpContextFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.GetAsync($"/api/Slides");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<SlideViewModel>>>(await response.Content.ReadAsStringAsync());
        }
    }
}
