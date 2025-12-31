using System.ComponentModel.DataAnnotations;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using ARAS.Models.User.ResponseModels;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllDropDownValuesResponseModel
    {
        public List<ResourceNameModel> ResourceNames { get; set; }
        public IList<DropDownValueModel> Status { get; set; } = [];
        public IList<DropDownValueModel> Category { get; set; } = [];
        public IList<DropDownValueModel> Project { get; set; } = [];
        public IList<DropDownValueModel> Network { get; set; } = [];
        public IList<DropDownValueModel> RNAndFeatureList { get; set; } = [];
    }
}
