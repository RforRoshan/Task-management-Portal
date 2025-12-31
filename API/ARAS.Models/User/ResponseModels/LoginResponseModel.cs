using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class LoginResponseModel
    {
        public string Token { get; set; } = string.Empty;
        public bool IsFirstTimeLogin { get; set; } = false;
    }
}
