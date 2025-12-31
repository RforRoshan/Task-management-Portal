using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Domain.Models.Task
{
    public class AddSubTaskWorkEntryResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public Guid WorkEntryUniqueId { get; set; } = Guid.NewGuid();
        public decimal EntryHrs { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }
        public long UserId { get; set; }

    }
    public class UpdateSubTaskWorkEntryRequest
    {
        public Guid WorkEntryUniqueId { get; set; }
        public decimal EntryHrs { get; set; }

    }
    public class UpdateSubTaskWorkEntryResponse
    {
        public bool IsError { get; set; }
        public decimal EntryHrs { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }

    }
}
