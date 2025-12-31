using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class UpdateWorkTimeRequestModel
    {
        public Guid WorkEntryUniqueId { get; set; }
        public decimal EntryHrs { get; set; }
    }
}
