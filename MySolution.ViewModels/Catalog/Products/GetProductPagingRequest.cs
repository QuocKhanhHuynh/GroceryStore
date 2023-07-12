using MySolution.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Products
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string? keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}
