using MySolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public DateTime OrderDate { set; get; }
        [Required]
        public string UserId { set; get; }
        [Required]
        public string ShipName { set; get; }
        [Required]
        public string ShipAddress { set; get; }
        [Required]
        public string ShipEmail { set; get; }
        [Required]
        public string ShipPhoneNumber { set; get; }
        [Required]
        public OrderStatus Status { set; get; }
        public User User { set; get; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
