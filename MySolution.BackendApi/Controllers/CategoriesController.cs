using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySolution.BackendApi.Data;
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Common;

namespace MySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _context;
        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Categories.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<CategoryViewModel>>(query));
        }
    }
}
