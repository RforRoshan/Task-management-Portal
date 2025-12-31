using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class ApiResult<T>
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T ResponseData { get; set; }
        public bool Status { get; set; }
    }
}
