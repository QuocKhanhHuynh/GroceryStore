using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySolution.BackendApi.Common;
using MySolution.BackendApi.Data;
using MySolution.BackendApi.Data.Entities;
using MySolution.Models.Catalog.ProductImage;
using MySolution.Models.Catalog.Products;
using MySolution.Models.Common;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace MySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _Context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        public ProductsController(MyDbContext context, IStorageService storageService, IConfiguration configuration)
        {
            _Context = context;
            _storageService = storageService;
            _configuration = configuration;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllProduct([FromQuery]GetProductPagingRequest request)
        {
            var query = from p in _Context.Products
                        join c in _Context.Categories on p.CategoryId equals c.Id
                        join pic in _Context.ProductImages on p.Id equals pic.ProductId
                        where pic.IsDefault == true
                        orderby p.Stock
                        select new { p, c, pic };
            if (!string.IsNullOrEmpty(request.keyword))
                query = query.Where(x => x.p.Name.Contains(request.keyword));
            if (request.CategoryId != null && request.CategoryId != 0)
                query = query.Where(x => request.CategoryId == x.c.Id);
            int total = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Descrestion = x.p.Descrestion,
                Price = x.p.Price,
                ViewCount = x.p.ViewCount,
                Stock = x.p.Stock,
                Category = x.c.Name,
                IsFeatured = x.p.IsFeatured,
                ThumbnailImage = x.pic.ImagePath,
                DateCreate = x.p.DateCreated

            }).ToListAsync();
            var pageResult = new PageResult<ProductViewModel>()
            {
                Items = data,
                TotalRecords = total,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };
            return Ok(new ApiSuccessResult<PageResult<ProductViewModel>>(pageResult));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var query = from p in _Context.Products
                        join c in _Context.Categories on p.CategoryId equals c.Id
                        join pic in _Context.ProductImages on p.Id equals pic.ProductId
                        where pic.IsDefault == true
                        select new { p, c, pic };
            var product = await query.FirstOrDefaultAsync(x => x.p.Id == productId);
            if (product == null)
                return BadRequest(new ApiErrorResult<bool>());
            var result = new ProductViewModel()
            {
                Id = product.p.Id,
                Name = product.p.Name,
                Descrestion = product.p.Descrestion,
                Price = product.p.Price,
                ViewCount = product.p.ViewCount,
                Stock = product.p.Stock,
                Category = product.c.Name,
                IsFeatured = product.p.IsFeatured,
                ThumbnailImage = product.pic.ImagePath,
                DateCreate = product.p.DateCreated
            };
            return Ok(new ApiSuccessResult<ProductViewModel>(result));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Descrestion = request.Descrestion,
                Price = request.Price,
                Stock = request.Stock,
                ViewCount = request.ViewCount,
                SeoAlias = request.SeoAlias,
                CategoryId = request.CategoryId,
                IsFeatured = request.IsFeatured
            };

            product.ProductImages = (request.ThumbnailImage1 != null || request.ThumbnailImage2 != null || request.ThumbnailImage3 != null || request.ThumbnailImage4 != null || request.ThumbnailImage5 != null) ?
                                    new List<ProductImage>() : null;
            if (request.ThumbnailImage1 != null)
            {
                product.ProductImages.Add(new ProductImage()
                {
                    Caption = request.Caption,
                    ImagePath = await this.SaveFile(request.ThumbnailImage1),
                    FileSize = request.ThumbnailImage1.Length,
                    IsDefault = true
                });
            }
            if (request.ThumbnailImage2 != null)
            {
                product.ProductImages.Add(new ProductImage()
                {
                    Caption = request.Caption,
                    ImagePath = await this.SaveFile(request.ThumbnailImage2),
                    FileSize = request.ThumbnailImage2.Length,
                    IsDefault = false
                });
            }
            if (request.ThumbnailImage3 != null)
            {
                product.ProductImages.Add(new ProductImage()
                {
                    Caption = request.Caption,
                    ImagePath = await this.SaveFile(request.ThumbnailImage3),
                    FileSize = request.ThumbnailImage3.Length,
                    IsDefault = false
                });
            }
            if (request.ThumbnailImage4 != null)
            {
                product.ProductImages.Add(new ProductImage()
                {
                    Caption = request.Caption,
                    ImagePath = await this.SaveFile(request.ThumbnailImage4),
                    FileSize = request.ThumbnailImage4.Length,
                    IsDefault = false
                });
            }
            if (request.ThumbnailImage5 != null)
            {
                product.ProductImages.Add(new ProductImage()
                {
                    Caption = request.Caption,
                    ImagePath = await this.SaveFile(request.ThumbnailImage5),
                    FileSize = request.ThumbnailImage5.Length,
                    IsDefault = false
                });
            }
            var r1 = await _Context.Products.AddAsync(product);
            await _Context.SaveChangesAsync();
            return Ok(new ApiSuccessResult<int>(product.Id));
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId,[FromForm] ProductUpdateRequest request)
        {
            var product = await _Context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return BadRequest(new ApiErrorResult<bool>());
            }
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.ViewCount = request.ViewCount;
            product.Descrestion = request.Descrestion;
            product.IsFeatured = request.IsFeatured;
            _Context.Products.Update(product);
            var result = await _Context.SaveChangesAsync() > 0;
            if (result)
            {
                return Ok(new ApiSuccessResult<bool>(true));
            }
            else
            {
                return BadRequest(new ApiErrorResult<bool>());
            }
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _Context.Products.FindAsync(productId);
            if (product == null)
                return BadRequest(new ApiErrorResult<bool>());
            _Context.Products.Remove(product);
            var Image = _Context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true);
            foreach (var i in Image)
            {
                await _storageService.DeleteFileAsync(i.ImagePath);
                _Context.ProductImages.Remove(i);
            }
            var result = await _Context.SaveChangesAsync() > 0;
            if (result)
            {
                return Ok(new ApiSuccessResult<bool>(true));
            }
            else
            {
                return BadRequest(new ApiErrorResult<bool>());
            }
        }

        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetListImage(int productId)
        {
            var Images = await _Context.ProductImages.Where(x => x.ProductId == productId).Select(x => new ProductImageViewModel()
            {
                Caption = x.Caption,
                FileSize = x.FileSize,
                ImagePath = x.ImagePath,
            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<ProductImageViewModel>>(Images));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var query = from p in _Context.Products
                        join c in _Context.Categories on p.CategoryId equals c.Id
                        join pic in _Context.ProductImages on p.Id equals pic.ProductId
                        where pic.IsDefault == true
                        select new { p, c, pic };
            var data = await query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Descrestion = x.p.Descrestion,
                Price = x.p.Price,
                ViewCount = x.p.ViewCount,
                Stock = x.p.Stock,
                Category = x.c.Name,
                IsFeatured = x.p.IsFeatured,
                ThumbnailImage = x.pic.ImagePath,
                DateCreate = x.p.DateCreated

            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<ProductViewModel>>(data));
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

    }
}
