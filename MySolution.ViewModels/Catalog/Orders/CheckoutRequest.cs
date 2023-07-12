using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Catalog.Orders
{
    public class CheckoutRequest
    {
        public string UserId { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public List<OrderDetail> Details { set; get; } = new List<OrderDetail>();
    }
}
