using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data.Entities
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string  ImagePath { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        [Required]
        public long FileSize { get; set; }
        public DateTime DateCeated { get; set; }
        public Product Product { get; set; }

    }
}
