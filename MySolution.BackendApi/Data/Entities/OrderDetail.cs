using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data.Entities
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        [Required]
        public int Quantity { set; get; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
