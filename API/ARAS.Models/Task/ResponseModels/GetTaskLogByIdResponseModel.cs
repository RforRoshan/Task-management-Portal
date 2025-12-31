using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Microsoft.EntityFrameworkCore;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetTaskLogByIdResponseModel
    {
        public IList<TasksLogModel> TasksLogs { get; set; } = [];
    }

    public class TasksLogModel
    {
        public string Log { get; set; }
        public long UserId { get; set; }
        public string Date { get; set; }
    }
}
