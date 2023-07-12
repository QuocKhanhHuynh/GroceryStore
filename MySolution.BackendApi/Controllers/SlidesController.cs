using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySolution.BackendApi.Data;
using MySolution.Models.Catalog.Slides;
using MySolution.Models.Common;

namespace MySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlidesController : ControllerBase
    {
        private readonly MyDbContext _context;
        public SlidesController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _context.Slides.Select(x => new SlideViewModel()
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name,
                Url = x.Url
            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<SlideViewModel>>(slides));
        }
        [HttpGet("create")]
        public async Task<IActionResult> creat(string name, string url, string img)
        {
            var result = _context.Slides.Add(new Data.Entities.Slide()
            {
                Image = img,
                Name = name,
                Url = url,
            });
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
