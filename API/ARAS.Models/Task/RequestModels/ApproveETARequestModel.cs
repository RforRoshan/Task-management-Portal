using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.Task.RequestModels
{
    public class ApproveETARequestModel
    {
        public Guid UniqueId { get; set; }
        public bool IsAll { get; set; }
    }
}
