using Microsoft.AspNetCore.Mvc;
using MySolution.Models.Catalog.ProductImage;
using MySolution.Models.Catalog.Products;
using MySolution.Models.Common;

namespace MySolution.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<ApiResult<PageResult<ProductViewModel>>> GetAllProduct(GetProductPagingRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<ProductViewModel>> GetProductById(int id);
        Task<ApiResult<bool>> Delete(ProductDeleteRequest request);
        Task<ApiResult<List<ProductViewModel>>> GetProduct();
        Task<ApiResult<List<ProductImageViewModel>>> GetListImage(int productId);
    }
}
