using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySolution.BackendApi.Data;
using MySolution.BackendApi.Data.Entities;
using MySolution.Data.Enums;
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Catalog.Orders;
using MySolution.Models.Catalog.Products;
using MySolution.Models.Common;

namespace MySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly MyDbContext _Context;
        private readonly UserManager<User> _userMangager;
        public OrdersController(MyDbContext context, UserManager<User> userMangager)
        {
            _Context = context;
            _userMangager = userMangager;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody]CheckoutRequest request)
        {
            var order = new Order()
            {
                OrderDate = DateTime.Now,
                ShipAddress = request.Address,
                ShipEmail = request.Email,
                ShipName = request.LastName + " " + request.FirstName,
                ShipPhoneNumber = request.PhoneNumber,
                Status = 0,
                UserId = request.UserId
            };
            order.OrderDetails = new List<Data.Entities.OrderDetail>();
            foreach (var item in request.Details)
            {
                var product = await _Context.Products.FirstAsync(x => x.Id == item.ProductId);
                product.Stock -= item.Quanlity;
                _Context.Products.Update(product);
                order.OrderDetails.Add(new Data.Entities.OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quanlity,
                });
            }
            await _Context.Orders.AddAsync(order);
            var result = (await _Context.SaveChangesAsync()) > 0;
            if (result)
            {
                return Ok(new ApiSuccessResult<bool>(true));
            }
            else
            {
                return BadRequest(new ApiErrorResult<bool>());
            }
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetOrderByUserId(string userName)
        {
            var users = _userMangager.Users;
            var user = await users.FirstOrDefaultAsync(x => x.UserName.Equals(userName));
            var Orders = await _Context.Orders.Where(x => x.UserId == user.Id).Select(x => new OrderViewModel()
            {
                Id = x.Id,
                Date = x.OrderDate,
                Status = (int)x.Status
            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<OrderViewModel>>(Orders));
        }
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var query = from o in _Context.OrderDetails
                        join p in _Context.Products on o.ProductId equals p.Id
                        join i in _Context.ProductImages on p.Id equals i.ProductId
                        where o.OrderId == id && i.IsDefault == true
                        select new { o, i, p };
            var orders = await query.Select(x => new OrderDetailViewModel()
            {
                Name = x.p.Name,
                Price = x.p.Price,
                Quanlity = x.o.Quantity,
                Image = x.i.ImagePath,
            }).ToListAsync();
            return Ok(new ApiSuccessResult<List<OrderDetailViewModel>>(orders));
        }
        [HttpGet]
        public async Task<IActionResult> GetPagingOrder([FromQuery] OrderPagingRequest request)
        {
            var query = await _Context.Orders.ToListAsync();
            if (request.OrderId != null)
            {
                query = query.Where(x => x.Id == request.OrderId).ToList();
            }
            if (request.Status == null && request.OrderId == null)
            {
                query = query.Where(x => x.Status != OrderStatus.Success).ToList();
            }
            if (request.Status != null)
            {
                query = query.Where(x => x.Status == (OrderStatus)request.Status).ToList();
            }
            var total = query.Count();
            var orders = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new OrderViewModel()
            {
                Id = x.Id,
                Date = x.OrderDate,
                Status = (int)x.Status,
                Address = x.ShipAddress,
                Name = x.ShipName,
            }).ToList();
            var result = new PageResult<OrderViewModel>()
            {
                Items = orders,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = total
            };
            return Ok(new ApiSuccessResult<PageResult<OrderViewModel>>(result));
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateStatusOrder([FromBody]UpdateStatusOrderRequest request)
        {
            var order = await _Context.Orders.FindAsync(request.Id);
            if (order == null)
            {
                return BadRequest(new ApiErrorResult<bool>());
            }
            order.Status = (OrderStatus)request.Status;
            _Context.Orders.Update(order);
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
    }
}
