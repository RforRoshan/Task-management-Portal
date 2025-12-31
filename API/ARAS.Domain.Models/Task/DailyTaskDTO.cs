using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Domain.Models.Task
{
    public class CurrentTaskDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public Guid TaskUniqueId { get; set; }
        public int TaskSeq { get; set; }
        public string Catagory { get; set; }
        public string TaskName { get; set; }
        public string SubProject { get; set; }
        public string Network { get; set; }
        public string Status { get; set; }
        public decimal TotalETA { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }
        public bool LastDayWork { get; set; } = false;
        public bool TodayDayWork { get; set; }
        public bool ItemToDiscuss { get; set; }
        public string EHrsToday { get; set; }
        public List<EHrsLastDTO> EHrsLast { get; set; } = [];
        public string MyComments { get; set; }
        public string ManagerComments { get; set; }
        public string Jira { get; set; }
        public bool IsApprove { get; set; }
        public Guid RNGuidId { get; set; }
        public string FixVersion { get; set; }
        public string MailTitled { get; set; }
    }

    public class EHrsLastDTO
    {
        public string FirstName { get; set; }
        public decimal WorksHrs { get; set; }
    }

    public class TodayWorkCurrentTasks
    {
        public Guid TaskUniqueId { get; set; }
        public string FirstName { get; set; }
        public decimal WorksHrs { get; set; }
    }

    public class DoneTaskDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public Guid TaskUniqueId { get; set; }
        public int TaskSeq { get; set; }
        public string Catagory { get; set; }
        public string TaskName { get; set; }
        public string SubProject { get; set; }
        public string Network { get; set; }
        public string Status { get; set; }
        public decimal TotalETA { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }
        public string MyComments { get; set; }
        public string ManagerComments { get; set; }
        public string Jira { get; set; }
        public string RNName { get; set; }
        public string FixVersion { get; set; }
        public string MailTitled { get; set; }
    }
    public class GetCurrentTaskByIdResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public Guid TaskUniqueId { get; set; }
        public int TaskSeq { get; set; }
        public string Catagory { get; set; }
        public string TaskName { get; set; }
        public string SubProject { get; set; }
        public string Network { get; set; }
        public string Status { get; set; }
        public decimal TotalETA { get; set; }
        public decimal UsedETA { get; set; }
        public decimal OtherUsedETA { get; set; }
        public bool LastDayWork { get; set; } = false;
        public bool TodayDayWork { get; set; }
        public bool ItemToDiscuss { get; set; }
        public string EHrsToday { get; set; }
        public List<EHrsLastDTO> EHrsLast { get; set; } = [];
        public string MyComments { get; set; }
        public string ManagerComments { get; set; }
        public string Jira { get; set; }
        public bool IsApprove { get; set; }
        public Guid RNGuidId { get; set; }
        public string FeatureName { get; set; }
        public string FixVersion { get; set; }
        public string RNComments { get; set; }
        public string MailTitled { get; set; }
    }

    public class ShowTaskToCopyDTO
    {
        public string FirstName { get; set; }
        public Guid TaskUniqueId { get; set; }
        public int TaskSeq { get; set; }
        public string TaskName { get; set; }
        public string SubProject { get; set; }
        public string Network { get; set; }
        public string Status { get; set; }
    }

    public class ShowSubTaskToCopyDTO
    {
        public int SubTaskSeq { get; set; }
        public string SubTaskName { get; set; }
        public decimal SubTaskETA { get; set; }
    }
}
