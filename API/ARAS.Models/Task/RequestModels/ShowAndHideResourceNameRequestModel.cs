using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Models.Task.ResponseModels;

namespace ARAS.Models.Task.RequestModels
{
    public class ShowAndHideResourceNameRequestModel
    {
        public Guid GuidId { get; set; }
        public bool IsShow { get; set; }
    }
}
