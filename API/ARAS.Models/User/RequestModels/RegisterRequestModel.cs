using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.RequestModels
{
    public class RegisterRequestModel
    {
        [StringLength(100)]
        public string Username { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; } 
        [StringLength(100)]
        public string MiddleName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; } 
        public long EmployeeNumber { get; set; }
        [StringLength(225)]
        public string Email { get; set; }
        public string IP { get; set; }
        public List<AccessRegisterModel> AccessDetails { get; set; }
    }

    public class AccessRegisterModel
    {
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
    }
}
