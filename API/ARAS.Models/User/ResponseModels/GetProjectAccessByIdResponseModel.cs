using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class GetProjectAccessByIdResponseModel
    {
        public List<ProjectAndRoleModel> ProjectAndRoles { get; set; } = [];
    }
    public class ProjectAndRoleModel
    {
        public int RoleId { get; set; }
        public int ProjectId { get; set; }
    }
}
