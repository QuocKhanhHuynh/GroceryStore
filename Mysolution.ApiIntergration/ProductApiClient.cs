using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySolution.Models.Catalog.ProductImage;
using MySolution.Models.Catalog.Products;
using MySolution.Models.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MySolution.ApiIntergration
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage1 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage1.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage1.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage1", request.ThumbnailImage1.FileName);
            }
            if (request.ThumbnailImage2 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage2.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage2.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage2", request.ThumbnailImage2.FileName);
            }
            if (request.ThumbnailImage3 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage3.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage3.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage3", request.ThumbnailImage3.FileName);
            }
            if (request.ThumbnailImage4 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage4.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage4.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage4", request.ThumbnailImage4.FileName);
            }
            if (request.ThumbnailImage5 != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage5.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage5.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "ThumbnailImage5", request.ThumbnailImage5.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            requestContent.Add(new StringContent(request.Price.ToString()), "Price");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(request.Descrestion.ToString()), "Descrestion");
            requestContent.Add(new StringContent(request.Caption.ToString()), "Caption");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "SeoAlias");
            requestContent.Add(new StringContent(request.IsFeatured.ToString()), "IsFeatured");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("api/Products", requestContent);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> Delete(ProductDeleteRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.DeleteAsync($"/api/Products/{request.Id}");
            if (responese.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await responese.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await responese.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PageResult<ProductViewModel>>> GetAllProduct(GetProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.GetAsync($"/api/Products/paging?keyword={request.keyword}&CategoryId={request.CategoryId}&PageIndex={request.PageIndex}&PageSize={request.PageSize}");
            return JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<ProductViewModel>>>(await responese.Content.ReadAsStringAsync());
        }
        
        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int productId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.GetAsync($"/api/Products/{productId}/images");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<ProductImageViewModel>>>(await responese.Content.ReadAsStringAsync());
        }
        
        public async Task<ApiResult<List<ProductViewModel>>> GetProduct()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var responese = await client.GetAsync($"/api/Products");
            return JsonConvert.DeserializeObject<ApiSuccessResult<List<ProductViewModel>>>(await responese.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<ProductViewModel>> GetProductById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.GetAsync($"/api/Products/{id}");
            return JsonConvert.DeserializeObject<ApiSuccessResult<ProductViewModel>>(await responese.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(request.Id.ToString()), "Id");
            requestContent.Add(new StringContent(request.Price.ToString()), "Price");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(request.Descrestion.ToString()), "Descrestion");
            requestContent.Add(new StringContent(request.IsFeatured.ToString()), "IsFeatured");
            requestContent.Add(new StringContent(request.ViewCount.ToString()), "ViewCount");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));
            var responese = await client.PutAsync($"api/Products/{request.Id}", requestContent);
            if (responese.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(await responese.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(await responese.Content.ReadAsStringAsync());
        }
    }
}