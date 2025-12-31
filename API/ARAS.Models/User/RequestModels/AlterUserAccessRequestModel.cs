using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class AlterUserAccessRequestModel
    {
        public List<UserAccessModel> UserAccess {  get; set; }
        public long UserId { get; set; }
    }
    public class UserAccessModel
    {
        public bool AcessHave { get; set; }
        public int ProjectId { get; set; }
        public string ProjectKey { get; set; }
        public int RoleId { get; set; }
    }
}
