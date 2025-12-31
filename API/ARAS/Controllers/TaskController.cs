using ARAS.Business.Services;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.Services;
using ARAS.Models.Task.RequestModels;
using ARAS.Models.Task.ResponseModels;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskServices _taskServices;

        public TaskController(ITaskServices taskServices)
        {
            _taskServices = taskServices;
        }
        [HttpPost("GetAllResourceNames")]
        [Authorize]
        public async Task<IActionResult> GetAllResourceNames(GetAllResourceNamesRequestModel requestModel)
        {
            ApiResult<GetAllResourceNamesResponseModel> responseModel = new ApiResult<GetAllResourceNamesResponseModel>();
            responseModel = await _taskServices.GetAllResourceNames(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("ShowAndHideResourceName")]
        [Authorize]
        public async Task<IActionResult> ShowAndHideResourceName(ShowAndHideResourceNameRequestModel requestModel)
        {
            ApiResult<ShowAndHideResourceNameResponseModel> responseModel = new ApiResult<ShowAndHideResourceNameResponseModel>();
            responseModel = await _taskServices.ShowAndHideResourceName(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetAllDropDownValuesByProjectKey")]
        [Authorize]
        public async Task<IActionResult> GetAllDropDownValuesByProjectKey(GetAllDropDownValuesByProjectKeyRequestModel requestModel)
        {
            ApiResult<GetAllDropDownValuesByProjectKeyResponseModel> responseModel = new ApiResult<GetAllDropDownValuesByProjectKeyResponseModel>();
            responseModel = await _taskServices.GetAllDropDownValuesByProjectKey(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("AddDropDownValues")]
        [Authorize]
        public async Task<IActionResult> AddDropDownValues(AddDropDownValuesRequestModel requestModel)
        {
            ApiResult<AddDropDownValuesResponseModel> responseModel = new ApiResult<AddDropDownValuesResponseModel>();
            responseModel = await _taskServices.AddDropDownValues(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("UpdateDropDownValues")]
        [Authorize]
        public async Task<IActionResult> UpdateDropDownValues(UpdateDropDownValuesRequestModel requestModel)
        {
            ApiResult<UpdateDropDownValuesResponseModel> responseModel = new ApiResult<UpdateDropDownValuesResponseModel>();
            responseModel = await _taskServices.UpdateDropDownValues(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("DeleteDropDownValues")]
        [Authorize]
        public async Task<IActionResult> DeleteDropDownValues(DeleteDropDownValuesRequestModel requestModel)
        {
            ApiResult<DeleteDropDownValuesResponseModel> responseModel = new ApiResult<DeleteDropDownValuesResponseModel>();
            responseModel = await _taskServices.DeleteDropDownValues(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetAllDropDownValues")]
        [Authorize]
        public async Task<IActionResult> GetAllDropDownValues(GetAllDropDownValuesRequestModel requestModel)
        {
            ApiResult<GetAllDropDownValuesResponseModel> responseModel = new ApiResult<GetAllDropDownValuesResponseModel>();
            responseModel = await _taskServices.GetAllDropDownValues(requestModel);
            return Ok(responseModel);
        }


        [HttpPost("GetAllCurrentTasks")]
        [Authorize]
        public async Task<IActionResult> GetAllCurrentTasks(GetAllCurrentTasksRequestModel requestModel)
        {
            ApiResult<GetAllCurrentTasksResponseModel> responseModel = new ApiResult<GetAllCurrentTasksResponseModel>();
            responseModel = await _taskServices.GetAllCurrentTasks(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetAllDoneTasks")]
        [Authorize]
        public async Task<IActionResult> GetAllDoneTasks(GetAllDoneTasksRequestModel requestModel)
        {
            ApiResult<GetAllDoneTasksResponseModel> responseModel = new ApiResult<GetAllDoneTasksResponseModel>();
            responseModel = await _taskServices.GetAllDoneTasks(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetTaskById")]
        [Authorize]
        public async Task<IActionResult> GetTaskById(GetTaskByIdRequestModel requestModel)
        {
            ApiResult<GetTaskByIdResponseModel> responseModel = new ApiResult<GetTaskByIdResponseModel>();
            responseModel = await _taskServices.GetTaskById(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("AddTask")]
        [Authorize]
        public async Task<IActionResult> AddTask(AddTaskRequestModel requestModel)
        {
            ApiResult<AddTaskResponseModel> responseModel = new ApiResult<AddTaskResponseModel>();
            responseModel = await _taskServices.AddTask(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("UpdateTask")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequestModel requestModel)
        {
            ApiResult<UpdateTaskResponseModel> responseModel = new ApiResult<UpdateTaskResponseModel>();
            responseModel = await _taskServices.UpdateTask(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("MiniUpdateTask")]
        [Authorize]
        public async Task<IActionResult> MiniUpdateTask(MiniUpdateTaskRequestModel requestModel)
        {
            ApiResult<MiniUpdateTaskResponseModel> responseModel = new ApiResult<MiniUpdateTaskResponseModel>();
            responseModel = await _taskServices.MiniUpdateTask(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("MoveTask")]
        [Authorize]
        public async Task<IActionResult> MoveTask(MoveTaskRequestModel requestModel)
        {
            ApiResult<MoveTaskResponseModel> responseModel = new ApiResult<MoveTaskResponseModel>();
            responseModel = await _taskServices.MoveTask(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetSubTasksById")]
        [Authorize]
        public async Task<IActionResult> GetSubTasksById(GetSubTasksByIdRequestModel requestModel)
        {
            ApiResult<GetSubTasksByIdResponseModel> responseModel = new ApiResult<GetSubTasksByIdResponseModel>();
            responseModel = await _taskServices.GetSubTasksById(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("AddSubTask")]
        [Authorize]
        public async Task<IActionResult> AddSubTask(AddSubTaskRequestModel requestModel)
        {
            ApiResult<AddSubTaskResponseModel> responseModel = new ApiResult<AddSubTaskResponseModel>();
            responseModel = await _taskServices.AddSubTask(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("UpdateSubTask")]
        [Authorize]
        public async Task<IActionResult> UpdateSubTask(UpdateSubTaskRequestModel requestModel)
        {
            ApiResult<UpdateSubTaskResponseModel> responseModel = new ApiResult<UpdateSubTaskResponseModel>();
            responseModel = await _taskServices.UpdateSubTask(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("AddWorkTime")]
        [Authorize]
        public async Task<IActionResult> AddWorkTime(AddWorkTimeRequestModel requestModel)
        {
            ApiResult<AddWorkTimeResponseModel> responseModel = new ApiResult<AddWorkTimeResponseModel>();
            responseModel = await _taskServices.AddWorkTime(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("UpdateWorkTime")]
        [Authorize]
        public async Task<IActionResult> UpdateWorkTime(UpdateWorkTimeRequestModel requestModel)
        {
            ApiResult<UpdateWorkTimeResponseModel> responseModel = new ApiResult<UpdateWorkTimeResponseModel>();
            responseModel = await _taskServices.UpdateWorkTime(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("GetTaskLogById")]
        [Authorize]
        public async Task<IActionResult> GetTaskLogById(GetTaskLogByIdRequestModel requestModel)
        {
            ApiResult<GetTaskLogByIdResponseModel> responseModel = new ApiResult<GetTaskLogByIdResponseModel>();
            responseModel = await _taskServices.GetTaskLogById(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetAllUseFullLink")]
        [Authorize]
        public async Task<IActionResult> GetAllUseFullLink(GetAllUseFullLinkRequestModel requestModel)
        {
            ApiResult<GetAllUseFullLinkResponseModel> responseModel = new ApiResult<GetAllUseFullLinkResponseModel>();
            responseModel = await _taskServices.GetAllUseFullLink(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("AddUseFullLink")]
        [Authorize]
        public async Task<IActionResult> AddUseFullLink(AddUseFullLinkRequestModel requestModel)
        {
            ApiResult<AddUseFullLinkResponseModel> responseModel = new ApiResult<AddUseFullLinkResponseModel>();
            responseModel = await _taskServices.AddUseFullLink(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("UpdateUseFullLink")]
        [Authorize]
        public async Task<IActionResult> UseFullLink(UpdateUseFullLinkRequestModel requestModel)
        {
            ApiResult<UpdateUseFullLinkResponseModel> responseModel = new ApiResult<UpdateUseFullLinkResponseModel>();
            responseModel = await _taskServices.UpdateUseFullLink(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("DeleteUseFullLink")]
        [Authorize]
        public async Task<IActionResult> DeleteUseFullLink(DeleteUseFullLinkRequestModel requestModel)
        {
            ApiResult<DeleteUseFullLinkResponseModel> responseModel = new ApiResult<DeleteUseFullLinkResponseModel>();
            responseModel = await _taskServices.DeleteUseFullLink(requestModel);
            return Ok(responseModel);
        }




        [HttpPost("GetAllTaskComment")]
        [Authorize]
        public async Task<IActionResult> GetAllTaskComment(GetAllTaskCommentRequestModel requestModel)
        {
            ApiResult<GetAllTaskCommentResponseModel> responseModel = new ApiResult<GetAllTaskCommentResponseModel>();
            responseModel = await _taskServices.GetAllTaskComment(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("AddTaskComment")]
        [Authorize]
        public async Task<IActionResult> AddTaskComment(AddTaskCommentRequestModel requestModel)
        {
            ApiResult<AddTaskCommentResponseModel> responseModel = new ApiResult<AddTaskCommentResponseModel>();
            responseModel = await _taskServices.AddTaskComment(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("UpdateTaskComment")]
        [Authorize]
        public async Task<IActionResult> TaskComment(UpdateTaskCommentRequestModel requestModel)
        {
            ApiResult<UpdateTaskCommentResponseModel> responseModel = new ApiResult<UpdateTaskCommentResponseModel>();
            responseModel = await _taskServices.UpdateTaskComment(requestModel);
            return Ok(responseModel);
        }
        [HttpPost("DeleteTaskComment")]
        [Authorize]
        public async Task<IActionResult> DeleteTaskComment(DeleteTaskCommentRequestModel requestModel)
        {
            ApiResult<DeleteTaskCommentResponseModel> responseModel = new ApiResult<DeleteTaskCommentResponseModel>();
            responseModel = await _taskServices.DeleteTaskComment(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("ApproveETA")]
        [Authorize]
        public async Task<IActionResult> ApproveETA(ApproveETARequestModel requestModel)
        {
            ApiResult<ApproveETAResponseModel> responseModel = new ApiResult<ApproveETAResponseModel>();
            responseModel = await _taskServices.ApproveETA(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetShowTasksToCopy")]
        [Authorize]
        public async Task<IActionResult> GetShowTasksToCopy(GetShowTasksToCopyRequestModel requestModel)
        {
            ApiResult<GetShowTasksToCopyResponseModel> responseModel = new ApiResult<GetShowTasksToCopyResponseModel>();
            responseModel = await _taskServices.GetShowTasksToCopy(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetShowSubTasksToCopy")]
        [Authorize]
        public async Task<IActionResult> GetShowSubTasksToCopy(GetShowSubTasksToCopyRequestModel requestModel)
        {
            ApiResult<GetShowSubTasksToCopyResponseModel> responseModel = new ApiResult<GetShowSubTasksToCopyResponseModel>();
            responseModel = await _taskServices.GetShowSubTasksToCopy(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetAllRNs")]
        [Authorize]
        public async Task<IActionResult> GetAllRNs(GetAllRNsRequestModel requestModel)
        {
            ApiResult<GetAllRNsResponseModel> responseModel = new ApiResult<GetAllRNsResponseModel>();
            responseModel = await _taskServices.GetAllRNs(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("AddRNAndFeature")]
        [Authorize]
        public async Task<IActionResult> AddRNAndFeature(AddRNAndFeatureRequestModel requestModel)
        {
            ApiResult<AddRNAndFeatureResponseModel> responseModel = new ApiResult<AddRNAndFeatureResponseModel>();
            responseModel = await _taskServices.AddRNAndFeature(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("UpdateRNAndFeature")]
        [Authorize]
        public async Task<IActionResult> UpdateRNAndFeature(UpdateRNAndFeatureRequestModel requestModel)
        {
            ApiResult<UpdateRNAndFeatureResponseModel> responseModel = new ApiResult<UpdateRNAndFeatureResponseModel>();
            responseModel = await _taskServices.UpdateRNAndFeature(requestModel);
            return Ok(responseModel);
        }

        [HttpPost("GetAllTasksByRNGuidId")]
        [Authorize]
        public async Task<IActionResult> GetAllTasksByRNGuidId(GetAllTasksByRNGuidIdRequestModel requestModel)
        {
            ApiResult<GetAllTasksByRNGuidIdResponseModel> responseModel = new ApiResult<GetAllTasksByRNGuidIdResponseModel>();
            responseModel = await _taskServices.GetAllTasksByRNGuidId(requestModel);
            return Ok(responseModel);
        }
    }
}
