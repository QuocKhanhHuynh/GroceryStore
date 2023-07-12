using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class LoginRequest
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public bool RememberMe { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
