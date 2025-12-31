using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Domain.Models.Task
{

    public class ApproveETARequest
    {
        public Guid UniqueId { get; set; } = new Guid();
        public bool IsAll { get; set; }
    }

    public class ApproveETAResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public decimal TotalETA { get; set; }
    }
    //public class AddTaskRequest
    //{
    //    public long UserId { get; set; }
    //    [StringLength(100)]
    //    public string UserName { get; set; }
    //    [StringLength(100)]
    //    public string Catagory { get; set; }
    //    public string TaskName { get; set; }
    //    [StringLength(50)]
    //    public string SubProject { get; set; }
    //    [StringLength(50)]
    //    public string ProjectKey { get; set; }
    //    public int ProjectId { get; set; }
    //    [StringLength(100)]
    //    public string Network { get; set; }
    //    [StringLength(100)]
    //    public string Status { get; set; }
    //    public bool ItemToDiscuss { get; set; }
    //    public string MyComments { get; set; }
    //    [StringLength(100)]
    //    public string Jira { get; set; }
    //}
    //public class AddTaskResponse
    //{
    //    public long UserId { get; set; }
    //    public string UserName { get; set; }
    //    public Guid TaskUniqueId { get; set; }
    //    public int TaskSeq { get; set; }
    //    public string Catagory { get; set; }
    //    public string TaskName { get; set; }
    //    public string SubProject { get; set; }
    //    public string Network { get; set; }
    //    public string Status { get; set; }
    //    public decimal TotalETA { get; set; }
    //    public decimal UsedETA { get; set; }
    //    public decimal OtherUsedETA { get; set; }
    //    public bool LastDayWork { get; set; } = false;
    //    public bool TodayDayWork { get; set; }
    //    public bool ItemToDiscuss { get; set; }
    //    public string EHrsToday { get; set; }
    //    public List<EHrsLastDTO> EHrsLast { get; set; } = [];
    //    public string MyComments { get; set; }
    //    public string ManagerComments { get; set; }
    //    public string Jira { get; set; }
    //}
}
