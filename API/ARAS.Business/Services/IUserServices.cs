using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;

namespace ARAS.Business.Services
{
    public interface IUserServices
    {
        public Task<ApiResult<LoginResponseModel>> Login(LoginRequestModel requestModel);
        public Task<ApiResult<FirstTimeUserResponseModel>> FirstTimeUser(FirstTimeUserRequestModel requestModel);
        public Task<ApiResult<RegisterResponseModel>> Register(RegisterRequestModel requestModel);

        public Task<ApiResult<ChangePasswardResponseModel>> ChangePassward(ChangePasswardRequestModel requestModel);
        public Task<ApiResult<ForgotPasswardLinkGenerateResponseModel>> ForgotPasswardLinkGenerate(ForgotPasswardLinkGenerateRequestModel requestModel);
        public Task<ApiResult<ForgotPasswardResponseModel>> ForgotPasswardReset(ForgotPasswardRequestModel requestModel);
        public Task<ApiResult<GetProjectAccessResponseModel>> GetProjectAccess(GetProjectAccessRequestModel requestModel);
        public Task<ApiResult<GetDashBoardDataResponseModel>> GetDashBoardData(GetDashBoardDataRequestModel requestModel);
        public Task<ApiResult<GetAllRoleByAccessResponseModel>> GetAllRoleByAccess(GetAllRoleByAccessRequestModel requestModel);
        public Task<ApiResult<GetAllUserResponseModel>> GetAllUser(GetAllUserRequestModel requestModel);
        public Task<ApiResult<GetProjectAccessByIdResponseModel>> GetProjectAccessById(GetProjectAccessByIdRequestModel requestModel);
        public Task<ApiResult<AlterUserAccessResponseModel>> AlterUserAccess(AlterUserAccessRequestModel requestModel);

    }
}
