using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class ForgotPasswardLinkGenerateRequestModel
    {
        public string EmailId { get; set; }
        public string LinkPrefix { get; set; }

    }
}
