using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ARAS.Domain.Models.Task;
using Microsoft.EntityFrameworkCore;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetShowTasksToCopyResponseModel
    {
        public IList<ShowTaskToCopyDTO> Tasks { get; set; } = [];
    }

}
