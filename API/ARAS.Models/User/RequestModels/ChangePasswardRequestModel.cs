using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class ChangePasswardRequestModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Source { get; set; }
    }
}
