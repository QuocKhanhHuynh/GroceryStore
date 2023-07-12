using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }

        public ApiSuccessResult(T result)
        {
            IsSuccessed = true;
            ResultObj = result;
        }
    }
}
