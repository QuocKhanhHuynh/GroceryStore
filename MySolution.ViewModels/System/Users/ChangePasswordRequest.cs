using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class ChangePasswordRequest
    {
        public string UserName { get; set; }
        [Display(Name ="Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }
        [Display(Name = "Mật khẩu hiện mới")]
        public string NewPassword { get; set; }
    }
}
