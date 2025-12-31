using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.Task.RequestModels
{
    public class GetAllTasksByRNGuidIdRequestModel
    {
        public string ProjectKey { get; set; }
        public Guid GuidId { get; set; }
    }
}
