using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data.Entities
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public bool IsAdmin { get; set; }
        public List<Order> Orders { get; set; }
    }
}
