using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Common
{
    public class PageResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; }
    }
}
