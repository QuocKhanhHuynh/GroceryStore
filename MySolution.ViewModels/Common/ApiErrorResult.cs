using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Models.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationError { get; set; }
        public ApiErrorResult()
        {
            IsSuccessed = false;
        }

        public ApiErrorResult(string[] validationError)
        {
            IsSuccessed = false;
            ValidationError = validationError;
        }

        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
