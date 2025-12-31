using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class AddWorkTimeRequestModel
    {
        public Guid SubTaskUniqueId { get; set; }
        public Guid TaskUniqueId { get; set; }
        public DateOnly EntryDate { get; set; }
        public decimal EntryHrs { get; set; }
    }
}
