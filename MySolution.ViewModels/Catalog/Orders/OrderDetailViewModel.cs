using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Orders
{
    public class OrderDetailViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Quanlity { get; set; }
    }
}
