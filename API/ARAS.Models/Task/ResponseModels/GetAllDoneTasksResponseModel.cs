using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ARAS.Domain.Models.Task;
using Microsoft.EntityFrameworkCore;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllDoneTasksResponseModel
    {
        public IList<DoneTaskDTO> Tasks { get; set; } = [];
    }

}
