using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.Task.ResponseModels
{
    public class UpdateWorkTimeResponseModel
    {
        public decimal EntryHrs { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }
    }
}
