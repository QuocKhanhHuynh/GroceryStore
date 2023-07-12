using MySolution.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.System.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
