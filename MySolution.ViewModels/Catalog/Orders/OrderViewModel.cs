using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
