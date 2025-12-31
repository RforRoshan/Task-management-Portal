using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class LoginRequestModel
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
    }
}
