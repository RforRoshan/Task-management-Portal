

using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using ARAS.Models.Task.RequestModels;
using ARAS.Models.Task.ResponseModels;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;

namespace ARAS.Business.Services
{
    public interface ITaskServices
    {
        Task<ApiResult<GetAllDropDownValuesByProjectKeyResponseModel>> GetAllDropDownValuesByProjectKey(GetAllDropDownValuesByProjectKeyRequestModel requestModel);
        public Task<ApiResult<ShowAndHideResourceNameResponseModel>> ShowAndHideResourceName(ShowAndHideResourceNameRequestModel requestModel);
        public Task<ApiResult<AddDropDownValuesResponseModel>> AddDropDownValues(AddDropDownValuesRequestModel requestModel);
        public Task<ApiResult<UpdateDropDownValuesResponseModel>> UpdateDropDownValues(UpdateDropDownValuesRequestModel requestModel);
        public Task<ApiResult<DeleteDropDownValuesResponseModel>> DeleteDropDownValues(DeleteDropDownValuesRequestModel requestModel);
        public Task<ApiResult<GetAllResourceNamesResponseModel>> GetAllResourceNames(GetAllResourceNamesRequestModel requestModel);
        public Task<ApiResult<GetAllDropDownValuesResponseModel>> GetAllDropDownValues(GetAllDropDownValuesRequestModel requestModel);
        public Task<ApiResult<GetAllCurrentTasksResponseModel>> GetAllCurrentTasks(GetAllCurrentTasksRequestModel requestModel);
        public Task<ApiResult<GetAllDoneTasksResponseModel>> GetAllDoneTasks(GetAllDoneTasksRequestModel requestModel);
        public Task<ApiResult<GetTaskByIdResponseModel>> GetTaskById(GetTaskByIdRequestModel requestModel);
        public Task<ApiResult<AddTaskResponseModel>> AddTask(AddTaskRequestModel requestModel);
        public Task<ApiResult<UpdateTaskResponseModel>> UpdateTask(UpdateTaskRequestModel requestModel);
        public Task<ApiResult<MoveTaskResponseModel>> MoveTask(MoveTaskRequestModel requestModel);
        public Task<ApiResult<GetSubTasksByIdResponseModel>> GetSubTasksById(GetSubTasksByIdRequestModel requestModel);
        public Task<ApiResult<AddSubTaskResponseModel>> AddSubTask(AddSubTaskRequestModel requestModel);
        public Task<ApiResult<UpdateSubTaskResponseModel>> UpdateSubTask(UpdateSubTaskRequestModel requestModel);
        public Task<ApiResult<AddWorkTimeResponseModel>> AddWorkTime(AddWorkTimeRequestModel requestModel);
        public Task<ApiResult<UpdateWorkTimeResponseModel>> UpdateWorkTime(UpdateWorkTimeRequestModel requestModel);
        public Task<ApiResult<GetTaskLogByIdResponseModel>> GetTaskLogById(GetTaskLogByIdRequestModel requestModel);
        public Task<ApiResult<GetAllUseFullLinkResponseModel>> GetAllUseFullLink(GetAllUseFullLinkRequestModel requestModel);
        public Task<ApiResult<AddUseFullLinkResponseModel>> AddUseFullLink(AddUseFullLinkRequestModel requestModel);
        public Task<ApiResult<UpdateUseFullLinkResponseModel>> UpdateUseFullLink(UpdateUseFullLinkRequestModel requestModel);
        public Task<ApiResult<DeleteUseFullLinkResponseModel>> DeleteUseFullLink(DeleteUseFullLinkRequestModel requestModel);

        public Task<ApiResult<GetAllTaskCommentResponseModel>> GetAllTaskComment(GetAllTaskCommentRequestModel requestModel);
        public Task<ApiResult<AddTaskCommentResponseModel>> AddTaskComment(AddTaskCommentRequestModel requestModel);
        public Task<ApiResult<UpdateTaskCommentResponseModel>> UpdateTaskComment(UpdateTaskCommentRequestModel requestModel);
        public Task<ApiResult<DeleteTaskCommentResponseModel>> DeleteTaskComment(DeleteTaskCommentRequestModel requestModel);

        public Task<ApiResult<MiniUpdateTaskResponseModel>> MiniUpdateTask(MiniUpdateTaskRequestModel requestModel);
        public Task<ApiResult<ApproveETAResponseModel>> ApproveETA(ApproveETARequestModel requestModel);
        public Task<ApiResult<GetShowTasksToCopyResponseModel>> GetShowTasksToCopy(GetShowTasksToCopyRequestModel requestModel);
        public Task<ApiResult<GetShowSubTasksToCopyResponseModel>> GetShowSubTasksToCopy(GetShowSubTasksToCopyRequestModel requestModel);
        public Task<ApiResult<GetAllRNsResponseModel>> GetAllRNs(GetAllRNsRequestModel requestModel);
        public Task<ApiResult<AddRNAndFeatureResponseModel>> AddRNAndFeature(AddRNAndFeatureRequestModel requestModel);
        public Task<ApiResult<UpdateRNAndFeatureResponseModel>> UpdateRNAndFeature(UpdateRNAndFeatureRequestModel requestModel);
        public Task<ApiResult<GetAllTasksByRNGuidIdResponseModel>> GetAllTasksByRNGuidId(GetAllTasksByRNGuidIdRequestModel requestModel);
    }
}
