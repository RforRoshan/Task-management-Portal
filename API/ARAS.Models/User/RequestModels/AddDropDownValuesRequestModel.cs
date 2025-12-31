using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class AddDropDownValuesRequestModel
    {
        public string ProjectKey { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
