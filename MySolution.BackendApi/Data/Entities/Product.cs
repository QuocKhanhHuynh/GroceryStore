
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int ViewCount { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Descrestion { get; set; }
        [Required]
        public string SeoAlias { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        public Category Category { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
