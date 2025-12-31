using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ARAS.Domain.Models.Task;
using Microsoft.EntityFrameworkCore;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllCurrentTasksResponseModel
    {
        public IList<CurrentTaskDTO> Tasks { get; set; } = [];
    }
}
