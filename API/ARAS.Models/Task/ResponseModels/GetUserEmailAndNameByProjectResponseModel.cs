using System.ComponentModel.DataAnnotations;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetUserEmailAndNameByProjectResponseModel
    {
        public List<string> EmailAndNames { get; set; }
    }
}
