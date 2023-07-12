using MySolution.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Orders
{
    public class OrderPagingRequest : PagingRequestBase
    {
        public int? OrderId { get; set; }
        public int? Status { get; set; }
    }
}
