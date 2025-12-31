
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;

namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllUseFullLinkResponseModel
    {
        public IList<UseFullLinkModel> UseFullLinks { get; set; } = [];
    }

}
