using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class UpdateSubTaskRequestModel
    {
        public Guid SubTaskUniqueId { get; set; }
        public int SubTaskSeq { get; set; }
        public string SubTaskName { get; set; }
        public string Remark { get; set; }
        public decimal SubTaskETA { get; set; }
        public string Status { get; set; }
        public bool IsColour { get; set; }
    }
}
