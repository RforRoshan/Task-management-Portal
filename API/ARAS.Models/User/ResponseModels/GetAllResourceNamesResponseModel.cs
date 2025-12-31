using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.User.ResponseModels
{
    public class GetAllResourceNamesResponseModel
    {
        public List<ResourceNameModel> ResourceNames { get; set; }
    }
    public class ResourceNameModel
    {
        public Guid GuidId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsShow { get; set; }
    }
}
