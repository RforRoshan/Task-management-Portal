using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class GetAllRoleByAccessResponseModel
    {
        public List<RoleModel> Roles { get; set; } = [];
    }
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

}
