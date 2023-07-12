
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Common;

namespace MySolution.ApiIntergration
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryViewModel>>> GetAll();
    }
}
