using Microsoft.AspNetCore.Mvc;
using MySolution.Models.Catalog.Categories;
using MySolution.Models.Catalog.Orders;
using MySolution.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.ApiIntergration
{
    public interface IOrderApiClient
    {
        public Task<ApiResult<bool>> AddOrder(CheckoutRequest request);
        public Task<ApiResult<List<OrderViewModel>>> GetOrderByUserId(string userName);
        public Task<ApiResult<List<OrderDetailViewModel>>> GetOrderDetail(int id);
        public Task<ApiResult<PageResult<OrderViewModel>>> GetPasingOrder(OrderPagingRequest request);
        public Task<ApiResult<bool>> UpdateStatusOrder(UpdateStatusOrderRequest request);
    }
}
