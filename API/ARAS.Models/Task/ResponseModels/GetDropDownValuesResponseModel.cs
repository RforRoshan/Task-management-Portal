using System.ComponentModel.DataAnnotations;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetDropDownValuesResponseModel
    {
        public IList<string> Resources { get; set; } = [];
        public IList<string> Status { get; set; } = [];
        public IList<string> Category { get; set; } = [];
        public IList<string> Project { get; set; } = [];
        public IList<string> Network { get; set; } = [];
    }
}
