using MySolution.Models.Catalog.Slides;
using MySolution.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.ApiIntergration
{
    public interface ISlideApiClient
    {
        Task<ApiResult<List<SlideViewModel>>> GetAll();
    }
}
