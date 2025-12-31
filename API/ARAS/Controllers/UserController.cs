using ARAS.Business.Services;
using ARAS.Business.Utility;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            ApiResult<LoginResponseModel> responseModel = new ApiResult<LoginResponseModel>();
            responseModel = await _userServices.Login(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("FirstTimeUser")]
        [AllowAnonymous]
        public async Task<IActionResult> FirstTimeUser(FirstTimeUserRequestModel requestModel)
        {
            ApiResult<FirstTimeUserResponseModel> responseModel = new ApiResult<FirstTimeUserResponseModel>();
            responseModel = await _userServices.FirstTimeUser(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("Register")]
        [Authorize]
        public async Task<IActionResult> Register(RegisterRequestModel requestModel)
        {
            ApiResult<RegisterResponseModel> responseModel = new ApiResult<RegisterResponseModel>();
            responseModel = await _userServices.Register(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("ChangePassward")]
        [Authorize]
        public async Task<IActionResult> ChangePassward(ChangePasswardRequestModel requestModel)
        {
            ApiResult<ChangePasswardResponseModel> responseModel = new ApiResult<ChangePasswardResponseModel>();
            responseModel = await _userServices.ChangePassward(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("ForgotPasswardLinkGenerate")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswardLinkGenerate(ForgotPasswardLinkGenerateRequestModel requestModel)
        {
            ApiResult<ForgotPasswardLinkGenerateResponseModel> responseModel = new ApiResult<ForgotPasswardLinkGenerateResponseModel>();
            responseModel = await _userServices.ForgotPasswardLinkGenerate(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("ForgotPasswardReset")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswardReset(ForgotPasswardRequestModel requestModel)
        {
            ApiResult<ForgotPasswardResponseModel> responseModel = new ApiResult<ForgotPasswardResponseModel>();
            responseModel = await _userServices.ForgotPasswardReset(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetProjectAccess")]
        [Authorize]
        public async Task<IActionResult> GetProjectAccess(GetProjectAccessRequestModel requestModel)
        {
            ApiResult<GetProjectAccessResponseModel> responseModel = new ApiResult<GetProjectAccessResponseModel>();
            responseModel = await _userServices.GetProjectAccess(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetDashBoardData")]
        [Authorize]
        public async Task<IActionResult> GetDashBoardData(GetDashBoardDataRequestModel requestModel)
        {
            ApiResult<GetDashBoardDataResponseModel> responseModel = new ApiResult<GetDashBoardDataResponseModel>();
            responseModel = await _userServices.GetDashBoardData(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetAllRoleByAccess")]
        [Authorize]
        public async Task<IActionResult> GetAllRoleByAccess(GetAllRoleByAccessRequestModel requestModel)
        {
            ApiResult<GetAllRoleByAccessResponseModel> responseModel = new ApiResult<GetAllRoleByAccessResponseModel>();
            responseModel = await _userServices.GetAllRoleByAccess(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUser(GetAllUserRequestModel requestModel)
        {
            ApiResult<GetAllUserResponseModel> responseModel = new ApiResult<GetAllUserResponseModel>();
            responseModel = await _userServices.GetAllUser(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetProjectAccessById")]
        [Authorize]
        public async Task<IActionResult> GetProjectAccessById(GetProjectAccessByIdRequestModel requestModel)
        {
            ApiResult<GetProjectAccessByIdResponseModel> responseModel = new ApiResult<GetProjectAccessByIdResponseModel>();
            responseModel = await _userServices.GetProjectAccessById(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("AlterUserAccess")]
        [Authorize]
        public async Task<IActionResult> AlterUserAccess(AlterUserAccessRequestModel requestModel)
        {
            ApiResult<AlterUserAccessResponseModel> responseModel = new ApiResult<AlterUserAccessResponseModel>();
            responseModel = await _userServices.AlterUserAccess(requestModel);
            return Ok(responseModel);
        }
    }
}
