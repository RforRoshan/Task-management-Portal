

namespace ARAS.Models.Task.ResponseModels
{
    public class GetSubTasksByIdResponseModel
    {
        public Guid TaskUniqueId { get; set; }
        public List<SubTaskModel> SubTasks { get; set; } = [];
        public List<SubTaskModel> OtherSubTasks { get; set; } = [];
    }
    public class SubTaskModel
    {
        public Guid SubTaskUniqueId { get; set; }
        public int SubTaskSeq { get; set; }
        public string SubTaskName { get; set; }
        public string Remark { get; set; }
        public decimal SubTaskETA { get; set; }
        public string Status { get; set; }
        public bool IsColour { get; set; }
        public bool IsApprove { get; set; }
        public List<WorkEntryDTO> WorkEntrys { get; set; } = [];
    }
    public class WorkEntryDTO
    {
        public Guid WorkEntryUniqueId { get; set; }
        public DateOnly EntryDate { get; set; }
        public decimal EntryHrs { get; set; }
        public long UserId { get; set; }
    }

}
