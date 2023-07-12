using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public string Descrestion { get; set; }
        public bool IsFeatured { get; set; }
    }
}
