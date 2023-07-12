using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class UpdateRequest
    {
        public string Id { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateTime Dob { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
