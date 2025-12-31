using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class MiniUpdateTaskRequestModel
    {
        public Guid TaskUniqueId { get; set; }
        
        public bool TodayDayWork { get; set; }
        public string EHrsToday { get; set; }
        public bool ItemToDiscuss { get; set; }
        public string ManagerComments { get; set; }
    }
}
