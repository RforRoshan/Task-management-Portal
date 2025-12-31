using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class GetAllUserResponseModel
    {
        public List<GetAllUserModel> userIdNames { get; set; } = [];
    }
    public class GetAllUserModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
