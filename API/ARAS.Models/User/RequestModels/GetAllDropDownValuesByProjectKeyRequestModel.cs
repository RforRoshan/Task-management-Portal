using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class GetAllDropDownValuesByProjectKeyRequestModel
    {
        public string ProjectKey { get; set; }
        public string Type { get; set; }
    }
}
