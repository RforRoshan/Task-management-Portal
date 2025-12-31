using System.ComponentModel.DataAnnotations;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;


namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllRNsResponseModel
    {
        public IList<DropDownValueModel> RNAndFeatureList { get; set; } = [];
    }
}
 