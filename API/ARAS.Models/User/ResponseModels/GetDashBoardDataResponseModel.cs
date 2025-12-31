using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class GetDashBoardDataResponseModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long UserRoleId { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public int ProjectId { get; set; }
    }
}
