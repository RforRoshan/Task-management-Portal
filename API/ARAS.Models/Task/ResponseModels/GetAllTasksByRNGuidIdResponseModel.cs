using System.ComponentModel.DataAnnotations;
using ARAS.Domain.Models.Task;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllTasksByRNGuidIdResponseModel
    {
        public IList<TaskForRN> Tasks { get; set; } = [];
    }
}
 