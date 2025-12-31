using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;

namespace ARAS.Models.User.ResponseModels
{
    public class GetProjectAccessResponseModel
    {
        public List<ProjectAccess> ProjectAccess { get; set; }
    }
}
