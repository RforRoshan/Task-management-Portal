
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using ARAS.Domain.Models.Task;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using ARAS.Infrastructure.Repository;
using ARAS.Infrastructure.Services;
using ARAS.Models.Task.RequestModels;
using ARAS.Models.Task.ResponseModels;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ARAS.Business.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskServices(ITaskRepository taskRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;

            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<ApiResult<GetAllCurrentTasksResponseModel>> GetAllCurrentTasks(GetAllCurrentTasksRequestModel requestModel)
        {
            ApiResult<GetAllCurrentTasksResponseModel> responseModel = new ApiResult<GetAllCurrentTasksResponseModel>();
            GetAllCurrentTasksResponseModel data = new GetAllCurrentTasksResponseModel();

            try
            {
                data.Tasks = await _taskRepository.GetAllCurrentTasks(requestModel.ProjectKey);

                List< TodayWorkCurrentTasks> todayWorkCurrentTasks = await _taskRepository.GetTodayWorkCurrentTasks(requestModel.ProjectKey, requestModel.DisplayDate);

                foreach (TodayWorkCurrentTasks todayWork in todayWorkCurrentTasks)
                {
                    var task = data.Tasks.FirstOrDefault(t => t.TaskUniqueId == todayWork.TaskUniqueId);
                    if (task != null)
                    {
                        EHrsLastDTO eHrsLastDTO = new EHrsLastDTO();
                        eHrsLastDTO.WorksHrs = todayWork.WorksHrs;
                        eHrsLastDTO.FirstName = todayWork.FirstName;
                        task.EHrsLast.Add(eHrsLastDTO);
                        task.ItemToDiscuss = true;
                        task.LastDayWork = true;
                    }
                }

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the current tasks.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetAllDoneTasksResponseModel>> GetAllDoneTasks(GetAllDoneTasksRequestModel requestModel)
        {
            ApiResult<GetAllDoneTasksResponseModel> responseModel = new ApiResult<GetAllDoneTasksResponseModel>();
            GetAllDoneTasksResponseModel data = new GetAllDoneTasksResponseModel();

            try
            {
                data.Tasks = await _taskRepository.GetAllDoneTasks(requestModel.ProjectKey);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the done tasks.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<AddTaskResponseModel>> AddTask(AddTaskRequestModel requestModel)
        {
            ApiResult<AddTaskResponseModel> responseModel = new ApiResult<AddTaskResponseModel>();
            AddTaskResponseModel data = new AddTaskResponseModel();

            try
            {
                DailyTask request = new DailyTask();

                request.UserId = requestModel.UserId;
                request.UserName = requestModel.UserName;
                request.Catagory = requestModel.Catagory;
                request.TaskName = requestModel.TaskName;
                request.SubProject = requestModel.SubProject;
                request.Network = requestModel.Network;
                request.Status = requestModel.Status;
                request.ProjectKey = requestModel.ProjectKey;
                request.ProjectId = requestModel.ProjectId;
                request.ItemToDiscuss = requestModel.ItemToDiscuss;
                request.MyComments = requestModel.MyComments;
                request.Jira = requestModel.Jira;

                DailyTask response = await _taskRepository.AddTask(request);

                data.UserId = request.UserId;
                data.UserName = request.UserName;
                data.Catagory = request.Catagory;
                data.TaskName = request.TaskName;
                data.SubProject = request.SubProject;
                data.Network = request.Network;
                data.Status = request.Status;
                data.ItemToDiscuss = request.ItemToDiscuss;
                data.MyComments = request.MyComments;
                data.Jira = request.Jira;
                data.TaskSeq = response.TaskSeq;
                data.TaskUniqueId = response.TaskUniqueId;
                data.LastDayWork = false;
                data.TodayDayWork = response.TodayDayWork;
                data.EHrsToday = response.EHrsToday;
                data.EHrsLast = [];
                data.ManagerComments = response.ManagerComments;
                data.TotalETA = 0;
                data.UsedETA = 0;
                data.OtherUsedETA = 0;
                data.IsApprove = false;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while adding task.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<MiniUpdateTaskResponseModel>> MiniUpdateTask(MiniUpdateTaskRequestModel requestModel)
        {

            ApiResult<MiniUpdateTaskResponseModel> responseModel = new ApiResult<MiniUpdateTaskResponseModel>();
            MiniUpdateTaskResponseModel data = new MiniUpdateTaskResponseModel();

            try
            {
                DailyTask request = new DailyTask();

                request.TaskUniqueId = requestModel.TaskUniqueId;
                request.TodayDayWork = requestModel.TodayDayWork;
                request.ItemToDiscuss = requestModel.ItemToDiscuss;
                request.ManagerComments = requestModel.ManagerComments;
                request.EHrsToday = requestModel.EHrsToday;

                data.Status = await _taskRepository.MiniUpdateTask(request);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating task mini data.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<MoveTaskResponseModel>> MoveTask(MoveTaskRequestModel requestModel)
        {

            ApiResult<MoveTaskResponseModel> responseModel = new ApiResult<MoveTaskResponseModel>();
            MoveTaskResponseModel data = new MoveTaskResponseModel();

            try
            {
                data.Status = await _taskRepository.MoveDailyTask(requestModel.TaskUniqueId, requestModel.IsAlive);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while moving task.";
                responseModel.Status = false;
            }
            return responseModel;
        }


        public async Task<ApiResult<GetTaskByIdResponseModel>> GetTaskById(GetTaskByIdRequestModel requestModel)
        {
            ApiResult<GetTaskByIdResponseModel> responseModel = new ApiResult<GetTaskByIdResponseModel>();
            GetTaskByIdResponseModel data = new GetTaskByIdResponseModel();

            try
            {
                GetCurrentTaskByIdResponse task = await _taskRepository.GetCurrentTaskById(requestModel.TaskUniqueId);

                if (task != null)
                {

                    data.TaskUniqueId = task.TaskUniqueId;
                    data.UserId = task.UserId;
                    data.UserName = task.UserName;
                    data.TaskSeq = task.TaskSeq;
                    data.Catagory = task.Catagory;
                    data.TaskName = task.TaskName;
                    data.SubProject = task.SubProject;
                    data.Network = task.Network;
                    data.Status = task.Status;
                    data.TotalETA = task.TotalETA;
                    data.UsedETA = task.UsedETA;
                    data.OtherUsedETA = task.OtherUsedETA;
                    data.LastDayWork = task.LastDayWork;
                    data.TodayDayWork = task.TodayDayWork;
                    data.ItemToDiscuss = task.ItemToDiscuss;
                    data.EHrsToday = task.EHrsToday;
                    data.EHrsLast = [];
                    data.MyComments = task.MyComments;
                    data.ManagerComments = task.ManagerComments;
                    data.Jira = task.Jira;
                    data.IsApprove = task.IsApprove;
                    data.RNGuidId = task.RNGuidId;
                    data.FeatureName = task.FeatureName;
                    data.FixVersion = task.FixVersion;
                    data.RNComments = task.RNComments;
                    data.MailTitled = task.MailTitled;

                    List<TodayWorkCurrentTasks> todayWorkCurrentTasks = await _taskRepository.GetTodayWorkCurrentTaskById(requestModel.TaskUniqueId, requestModel.DisplayDate);

                    foreach (TodayWorkCurrentTasks todayWork in todayWorkCurrentTasks)
                    {
                        EHrsLastDTO eHrsLastDTO = new EHrsLastDTO();
                        eHrsLastDTO.WorksHrs = todayWork.WorksHrs;
                        eHrsLastDTO.FirstName = todayWork.FirstName;
                        data.EHrsLast.Add(eHrsLastDTO);
                        data.ItemToDiscuss = true;
                        data.LastDayWork = true;
                    }

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "Unable to find the tasks.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the current tasks.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateTaskResponseModel>> UpdateTask(UpdateTaskRequestModel requestModel)
        {

            ApiResult<UpdateTaskResponseModel> responseModel = new ApiResult<UpdateTaskResponseModel>();
            UpdateTaskResponseModel data = new UpdateTaskResponseModel();

            try
            {
                DailyTask dailyTask = new DailyTask();

                dailyTask.TaskUniqueId = requestModel.TaskUniqueId;
                dailyTask.UserId = requestModel.UserId;
                dailyTask.UserName = requestModel.UserName;
                dailyTask.Catagory = requestModel.Catagory;
                dailyTask.TaskName = requestModel.TaskName;
                dailyTask.SubProject = requestModel.SubProject;
                dailyTask.SubProject = requestModel.SubProject;
                dailyTask.Network = requestModel.Network;
                dailyTask.Status = requestModel.Status;
                dailyTask.ItemToDiscuss = requestModel.ItemToDiscuss;
                dailyTask.MyComments = requestModel.MyComments;
                dailyTask.Jira = requestModel.Jira;

                dailyTask.RNGuidId = requestModel.RNGuidId;
                dailyTask.FeatureName = requestModel.FeatureName;
                dailyTask.FixVersion = requestModel.FixVersion;
                dailyTask.RNComments = requestModel.RNComments;
                dailyTask.MailTitled = requestModel.MailTitled;


                dailyTask = await _taskRepository.UpdateTask(dailyTask);

                data.UserName = dailyTask.UserName;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetSubTasksByIdResponseModel>> GetSubTasksById(GetSubTasksByIdRequestModel requestModel)
        {
            ApiResult<GetSubTasksByIdResponseModel> responseModel = new ApiResult<GetSubTasksByIdResponseModel>();
            GetSubTasksByIdResponseModel data = new GetSubTasksByIdResponseModel();

            List<SubTaskModel> selfSubTasks= new List<SubTaskModel>();
            List<SubTaskModel> otherSubTasks = new List<SubTaskModel>();
            //List<WorkEntryModel> workEntrysModel = new List<WorkEntryModel>();

            try
            {
                List<SubTask> subTasks = await _taskRepository.GetSubTasksById(requestModel.TaskUniqueId);

                List<SubTaskWorkEntry> subTaskWorkEntries = await _taskRepository.GetSubTaskWorkEntryById(requestModel.TaskUniqueId);

                // Group work entries by SubTaskUniqueId for O(1) lookup
                var workEntryLookup = subTaskWorkEntries.GroupBy(x => x.SubTaskUniqueId).ToDictionary(g => g.Key, g => g.ToList());

                foreach (SubTask subTask in subTasks)
                {
                    SubTaskModel subTaskModel = new SubTaskModel
                    {
                        SubTaskUniqueId = subTask.SubTaskUniqueId,
                        SubTaskName = subTask.SubTaskName,
                        SubTaskSeq = subTask.SubTaskSeq,
                        SubTaskETA = subTask.SubTaskETA,
                        Remark = subTask.Remark,
                        Status = subTask.Status,
                        IsColour = subTask.IsColour,
                        IsApprove = subTask.IsApprove
                    };
                    // Fast lookup
                    if (workEntryLookup.TryGetValue(subTask.SubTaskUniqueId, out var entries))
                    {
                        subTaskModel.WorkEntrys = entries.Select(e => new WorkEntryDTO
                        {
                            WorkEntryUniqueId = e.WorkEntryUniqueId,
                            EntryDate = e.EntryDate,
                            EntryHrs = e.EntryHrs,
                            UserId = e.UserId
                        }).ToList();
                    }
                    else
                    {
                        subTaskModel.WorkEntrys = new List<WorkEntryDTO>();
                    }
                    if (subTask.IsSelf)
                    {
                        selfSubTasks.Add(subTaskModel);
                    }
                    else
                    {
                        otherSubTasks.Add(subTaskModel);
                    }
                }

                data.TaskUniqueId = requestModel.TaskUniqueId;
                data.SubTasks = selfSubTasks;
                data.OtherSubTasks = otherSubTasks;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the sub tasks.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<AddSubTaskResponseModel>> AddSubTask(AddSubTaskRequestModel requestModel)
        {

            ApiResult<AddSubTaskResponseModel> responseModel = new ApiResult<AddSubTaskResponseModel>();
            AddSubTaskResponseModel data = new AddSubTaskResponseModel();

            try
            {
                SubTask subTask = new SubTask();
                subTask.TaskUniqueId = requestModel.TaskUniqueId;
                subTask.SubTaskSeq = requestModel.SubTaskSeq;
                subTask.SubTaskName = requestModel.SubTaskName;
                subTask.Remark = requestModel.Remark;
                subTask.SubTaskETA = requestModel.SubTaskETA;
                subTask.Status = requestModel.Status;
                subTask.IsColour = requestModel.IsColour;
                subTask.IsSelf = requestModel.IsSelf;

                decimal totalETA = await _taskRepository.AddSubTask(subTask);

                data.SubTaskUniqueId = subTask.SubTaskUniqueId;
                data.TotalETA = totalETA;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while adding subtask.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateSubTaskResponseModel>> UpdateSubTask(UpdateSubTaskRequestModel requestModel)
        {

            ApiResult<UpdateSubTaskResponseModel> responseModel = new ApiResult<UpdateSubTaskResponseModel>();
            UpdateSubTaskResponseModel data = new UpdateSubTaskResponseModel();

            try
            {
                SubTask subTask = new SubTask();
                subTask.SubTaskUniqueId = requestModel.SubTaskUniqueId;
                subTask.SubTaskSeq = requestModel.SubTaskSeq;
                subTask.SubTaskName = requestModel.SubTaskName;
                subTask.Remark = requestModel.Remark;
                subTask.SubTaskETA = requestModel.SubTaskETA;
                subTask.Status = requestModel.Status;
                subTask.IsColour = requestModel.IsColour;

                decimal totalETA = await _taskRepository.UpdateSubTask(subTask);

                data.TotalETA = totalETA;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating subtask.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<AddWorkTimeResponseModel>> AddWorkTime(AddWorkTimeRequestModel requestModel)
        {

            ApiResult<AddWorkTimeResponseModel> responseModel = new ApiResult<AddWorkTimeResponseModel>();
            AddWorkTimeResponseModel data = new AddWorkTimeResponseModel();

            try
            {
                SubTaskWorkEntry subTaskWorkEntry = new SubTaskWorkEntry();

                subTaskWorkEntry.SubTaskUniqueId = requestModel.SubTaskUniqueId;
                subTaskWorkEntry.TaskUniqueId = requestModel.TaskUniqueId;
                subTaskWorkEntry.EntryDate = requestModel.EntryDate;
                subTaskWorkEntry.EntryHrs = requestModel.EntryHrs;

                AddSubTaskWorkEntryResponse response = await _taskRepository.AddSubTaskWorkEntry(subTaskWorkEntry);

                if (response.IsError)
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = response.ErrorMessage;
                    responseModel.Status = false;
                }
                else
                {
                    data.WorkEntryUniqueId = response.WorkEntryUniqueId;
                    data.EntryHrs = response.EntryHrs;
                    data.UsedETA = response.UsedETA;
                    data.OtherUsedETA = response.OtherUsedETA;
                    data.UserId = response.UserId;

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
                    
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while adding work log.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<UpdateWorkTimeResponseModel>> UpdateWorkTime(UpdateWorkTimeRequestModel requestModel)
        {
            ApiResult<UpdateWorkTimeResponseModel> responseModel = new ApiResult<UpdateWorkTimeResponseModel>();
            UpdateWorkTimeResponseModel data = new UpdateWorkTimeResponseModel();

            try
            {
                UpdateSubTaskWorkEntryRequest request = new UpdateSubTaskWorkEntryRequest();
                request.WorkEntryUniqueId = requestModel.WorkEntryUniqueId;
                request.EntryHrs = requestModel.EntryHrs;

                UpdateSubTaskWorkEntryResponse response = await _taskRepository.UpdateSubTaskWorkEntry(request);
                if (response.IsError)
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "You can update only your log time, not others.";
                    responseModel.Status = false;
                }
                else
                {
                    data.EntryHrs = response.EntryHrs;
                    data.UsedETA = response.UsedETA;
                    data.OtherUsedETA = response.OtherUsedETA;

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating work log.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<ApproveETAResponseModel>> ApproveETA(ApproveETARequestModel requestModel)
        {
            ApiResult<ApproveETAResponseModel> responseModel = new ApiResult<ApproveETAResponseModel>();
            ApproveETAResponseModel data = new ApproveETAResponseModel();

            try
            {
                ApproveETARequest request = new ApproveETARequest();
                request.UniqueId = requestModel.UniqueId;
                request.IsAll = requestModel.IsAll;

                ApproveETAResponse response = await _taskRepository.ApproveETA(request);
                if (response.IsError)
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = response.ErrorMessage;
                    responseModel.Status = false;
                }
                else
                {
                    data.TotalETA = response.TotalETA;

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while approving task.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetShowTasksToCopyResponseModel>> GetShowTasksToCopy(GetShowTasksToCopyRequestModel requestModel)
        {
            ApiResult<GetShowTasksToCopyResponseModel> responseModel = new ApiResult<GetShowTasksToCopyResponseModel>();
            GetShowTasksToCopyResponseModel data = new GetShowTasksToCopyResponseModel();

            try
            {
                data.Tasks = await _taskRepository.GetTasksToCopy(requestModel.ProjectKey, requestModel.IsAlive);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the tasks for copy.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetShowSubTasksToCopyResponseModel>> GetShowSubTasksToCopy(GetShowSubTasksToCopyRequestModel requestModel)
        {
            ApiResult<GetShowSubTasksToCopyResponseModel> responseModel = new ApiResult<GetShowSubTasksToCopyResponseModel>();
            GetShowSubTasksToCopyResponseModel data = new GetShowSubTasksToCopyResponseModel();

            try
            {
                data.SubTasks = await _taskRepository.GetSubTasksToCopy(requestModel.TaskUniqueId);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the Subtasks for copy.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<GetAllRNsResponseModel>> GetAllRNs(GetAllRNsRequestModel requestModel)
        {
            ApiResult<GetAllRNsResponseModel> responseModel = new ApiResult<GetAllRNsResponseModel>();
            GetAllRNsResponseModel data = new GetAllRNsResponseModel();

            try
            {
                data.RNAndFeatureList = await _taskRepository.RNAndFeatureListByProjectKey(requestModel.ProjectKey);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the RN list.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<AddRNAndFeatureResponseModel>> AddRNAndFeature(AddRNAndFeatureRequestModel requestModel)
        {
            ApiResult<AddRNAndFeatureResponseModel> responseModel = new ApiResult<AddRNAndFeatureResponseModel>();
            AddRNAndFeatureResponseModel data = new AddRNAndFeatureResponseModel();

            try
            {
                RNAndFeature rNAndFeature = new RNAndFeature();
                rNAndFeature.ProjectKey = requestModel.ProjectKey;
                rNAndFeature.RN = requestModel.RN;


                rNAndFeature = await _taskRepository.AddRNAndFeature(rNAndFeature);
                data.GuidId = rNAndFeature.GuidId;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to add new RN.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateRNAndFeatureResponseModel>> UpdateRNAndFeature(UpdateRNAndFeatureRequestModel requestModel)
        {
            ApiResult<UpdateRNAndFeatureResponseModel> responseModel = new ApiResult<UpdateRNAndFeatureResponseModel>();
            UpdateRNAndFeatureResponseModel data = new UpdateRNAndFeatureResponseModel();

            try
            {
                await _taskRepository.UpdateRNAndFeature(requestModel.GuidId, requestModel.RN);
                data.GuidId = requestModel.GuidId;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to update new RN.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetAllTasksByRNGuidIdResponseModel>> GetAllTasksByRNGuidId(GetAllTasksByRNGuidIdRequestModel requestModel)
        {
            ApiResult<GetAllTasksByRNGuidIdResponseModel> responseModel = new ApiResult<GetAllTasksByRNGuidIdResponseModel>();
            GetAllTasksByRNGuidIdResponseModel data = new GetAllTasksByRNGuidIdResponseModel();

            try
            {
                data.Tasks = await _taskRepository.GetAllTasksByRNGuidId(requestModel.ProjectKey, requestModel.GuidId);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the rn details.";
                responseModel.Status = false;
            }
            return responseModel;
        }















































        public async Task<ApiResult<GetAllResourceNamesResponseModel>> GetAllResourceNames(GetAllResourceNamesRequestModel requestModel)
        {
            ApiResult<GetAllResourceNamesResponseModel> responseModel = new ApiResult<GetAllResourceNamesResponseModel>();
            GetAllResourceNamesResponseModel data = new GetAllResourceNamesResponseModel();
            List <ResourceNameModel> resourceNameModels = new List <ResourceNameModel>();

            try
            {
                Project project = await _userRepository.ProjectByKey(requestModel.ProjectKey);
                List<ResourceName> resourceNames = await _userRepository.GetAllResourceNamesByProjectKey(project.ProjectId);

                foreach (ResourceName resourceName in resourceNames)
                {
                    ResourceNameModel resourceNameModel = new ResourceNameModel();
                    resourceNameModel.UserName = resourceName.UserName;
                    resourceNameModel.UserId = resourceName.UserId;
                    resourceNameModel.Name = resourceName.Name;
                    resourceNameModel.LastName = resourceName.LastName;
                    resourceNameModel.IsShow = resourceName.IsShow;
                    resourceNameModel.GuidId = resourceName.GuidId;
                    resourceNameModels.Add(resourceNameModel);

                }

                data.ResourceNames = resourceNameModels;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<ShowAndHideResourceNameResponseModel>> ShowAndHideResourceName(ShowAndHideResourceNameRequestModel requestModel)
        {
            ApiResult<ShowAndHideResourceNameResponseModel> responseModel = new ApiResult<ShowAndHideResourceNameResponseModel>();
            ShowAndHideResourceNameResponseModel data = new ShowAndHideResourceNameResponseModel();
            ResourceName resourceName = new ResourceName();
            resourceName.GuidId = requestModel.GuidId;
            resourceName.IsShow = requestModel.IsShow;

            try
            {
                data.Status = await _userRepository.UpdateResourceName(resourceName);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = data.Status;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetAllDropDownValuesByProjectKeyResponseModel>> GetAllDropDownValuesByProjectKey(GetAllDropDownValuesByProjectKeyRequestModel requestModel)
        {

            ApiResult<GetAllDropDownValuesByProjectKeyResponseModel> responseModel = new ApiResult<GetAllDropDownValuesByProjectKeyResponseModel>();
            GetAllDropDownValuesByProjectKeyResponseModel data = new GetAllDropDownValuesByProjectKeyResponseModel();

            try
            {

                List<DropDownValueModel> dropDownValueModels = await _userRepository.GetAllDropDownValuesByProjectKey(requestModel.ProjectKey, requestModel.Type);
                data.DropDownValueModels = dropDownValueModels;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<AddDropDownValuesResponseModel>> AddDropDownValues(AddDropDownValuesRequestModel requestModel)
        {

            ApiResult<AddDropDownValuesResponseModel> responseModel = new ApiResult<AddDropDownValuesResponseModel>();
            AddDropDownValuesResponseModel data = new AddDropDownValuesResponseModel();

            try
            {

                DropDownValue dropDownValue = new DropDownValue();
                dropDownValue.Value = requestModel.Value;
                dropDownValue.ProjectKey = requestModel.ProjectKey;
                dropDownValue.Type = requestModel.Type;
                dropDownValue =  await _userRepository.AddDropDownValue(dropDownValue);

                data.GuidId = dropDownValue.GuidId;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateDropDownValuesResponseModel>> UpdateDropDownValues(UpdateDropDownValuesRequestModel requestModel)
        {

            ApiResult<UpdateDropDownValuesResponseModel> responseModel = new ApiResult<UpdateDropDownValuesResponseModel>();
            UpdateDropDownValuesResponseModel data = new UpdateDropDownValuesResponseModel();

            try
            {
                DropDownValue dropDownValue = new DropDownValue();
                dropDownValue.GuidId = requestModel.GuidId;
                dropDownValue.Value = requestModel.Value;
                data.Status = await _userRepository.UpdateDropDownValue(dropDownValue);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = data.Status;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<DeleteDropDownValuesResponseModel>> DeleteDropDownValues(DeleteDropDownValuesRequestModel requestModel)
        {

            ApiResult<DeleteDropDownValuesResponseModel> responseModel = new ApiResult<DeleteDropDownValuesResponseModel>();
            DeleteDropDownValuesResponseModel data = new DeleteDropDownValuesResponseModel();

            try
            {
                data.Status = await _userRepository.DeleteDropDownValue(requestModel.GuidId);
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = data.Status;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while deleting.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<GetAllDropDownValuesResponseModel>> GetAllDropDownValues(GetAllDropDownValuesRequestModel requestModel)
        {
            ApiResult<GetAllDropDownValuesResponseModel> responseModel = new ApiResult<GetAllDropDownValuesResponseModel>();
            GetAllDropDownValuesResponseModel data = new GetAllDropDownValuesResponseModel();
            List<ResourceNameModel> resourceNameModels = new List<ResourceNameModel>();

            try
            {
                Project project = await _userRepository.ProjectByKey(requestModel.ProjectKey);
                List<ResourceName> resourceNames = await _userRepository.GetAllResourceNamesByProjectKey(project.ProjectId);

                foreach (ResourceName resourceName in resourceNames)
                {
                    ResourceNameModel resourceNameModel = new ResourceNameModel();
                    resourceNameModel.UserName = resourceName.UserName;
                    resourceNameModel.UserId = resourceName.UserId;
                    resourceNameModel.Name = resourceName.Name;
                    resourceNameModel.LastName = resourceName.LastName;
                    resourceNameModel.IsShow = resourceName.IsShow;
                    resourceNameModel.GuidId = resourceName.GuidId;
                    resourceNameModels.Add(resourceNameModel);

                }

                data.ResourceNames = resourceNameModels;

                data.Status = await _userRepository.GetAllDropDownValuesByProjectKey(requestModel.ProjectKey, "status");
                data.Category = await _userRepository.GetAllDropDownValuesByProjectKey(requestModel.ProjectKey, "category");
                data.Project = await _userRepository.GetAllDropDownValuesByProjectKey(requestModel.ProjectKey, "project");
                data.Network = await _userRepository.GetAllDropDownValuesByProjectKey(requestModel.ProjectKey, "network");
                data.RNAndFeatureList = await _taskRepository.RNAndFeatureListByProjectKey(requestModel.ProjectKey);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        

        

        

        

        


        

        


        public async Task<ApiResult<GetTaskLogByIdResponseModel>> GetTaskLogById(GetTaskLogByIdRequestModel requestModel)
        {
            ApiResult<GetTaskLogByIdResponseModel> responseModel = new ApiResult<GetTaskLogByIdResponseModel>();
            GetTaskLogByIdResponseModel data = new GetTaskLogByIdResponseModel();

            List<TasksLogModel> tasksLogs = new List<TasksLogModel>();
            try
            {
                List<DailyTasksLog> dailyTasksLogs = await _userRepository.GetTaskLogById(requestModel.TaskUniqueId);



                foreach (DailyTasksLog dailyTasksLog in dailyTasksLogs)
                {
                    TasksLogModel tasksLog = new TasksLogModel();
                    tasksLog.Log = dailyTasksLog.Log;
                    tasksLog.UserId = dailyTasksLog.UserId;
                    tasksLog.Date = dailyTasksLog.Date.ToString("dd/MM/yyyy hh:mm:ss tt");

                    tasksLogs.Add(tasksLog);
                }
                data.TasksLogs = tasksLogs;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the log task history";
                responseModel.Status = false;
            }
            return responseModel;
        }








        #region UseFullLinkMethods
        public async Task<ApiResult<GetAllUseFullLinkResponseModel>> GetAllUseFullLink(GetAllUseFullLinkRequestModel requestModel)
        {
            ApiResult<GetAllUseFullLinkResponseModel> responseModel = new ApiResult<GetAllUseFullLinkResponseModel>();
            GetAllUseFullLinkResponseModel data = new GetAllUseFullLinkResponseModel();

            List<UseFullLinkModel> useFullLinkModels = new List<UseFullLinkModel>();
            try
            {
                List<UseFullLink> useFullLinks = await _taskRepository.GetAllUseFullLinkByProjectKey(requestModel.ProjectKey);

                foreach (UseFullLink useFullLink in useFullLinks)
                {
                    UseFullLinkModel useFullLinkModel = new UseFullLinkModel();
                    useFullLinkModel.GuidId = useFullLink.GuidId;
                    useFullLinkModel.LinkURL = useFullLink.LinkURL;
                    useFullLinkModel.LinkName = useFullLink.LinkName;

                    useFullLinkModels.Add(useFullLinkModel);
                }
                data.UseFullLinks = useFullLinkModels;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the useFullLinks";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<AddUseFullLinkResponseModel>> AddUseFullLink(AddUseFullLinkRequestModel requestModel)
        {
            ApiResult<AddUseFullLinkResponseModel> responseModel = new ApiResult<AddUseFullLinkResponseModel>();
            AddUseFullLinkResponseModel data = new AddUseFullLinkResponseModel();

            try
            {
                UseFullLink useFullLink = new UseFullLink();

                useFullLink.ProjectKey = requestModel.ProjectKey;
                useFullLink.LinkURL = requestModel.LinkURL;
                useFullLink.LinkName = requestModel.LinkName;

                useFullLink = await _taskRepository.AddUseFullLink(useFullLink);

                data.GuidId = useFullLink.GuidId;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while adding the useFullLink.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateUseFullLinkResponseModel>> UpdateUseFullLink(UpdateUseFullLinkRequestModel requestModel)
        {
            ApiResult<UpdateUseFullLinkResponseModel> responseModel = new ApiResult<UpdateUseFullLinkResponseModel>();
            UpdateUseFullLinkResponseModel data = new UpdateUseFullLinkResponseModel();

            try
            {
                UseFullLink useFullLink = new UseFullLink();

                useFullLink.GuidId = requestModel.GuidId;
                useFullLink.LinkURL = requestModel.LinkURL;
                useFullLink.LinkName = requestModel.LinkName;

                useFullLink = await _taskRepository.UpdateUseFullLink(useFullLink);

                data.Status = true;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating the useFullLink.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<DeleteUseFullLinkResponseModel>> DeleteUseFullLink(DeleteUseFullLinkRequestModel requestModel)
        {
            ApiResult<DeleteUseFullLinkResponseModel> responseModel = new ApiResult<DeleteUseFullLinkResponseModel>();
            DeleteUseFullLinkResponseModel data = new DeleteUseFullLinkResponseModel();

            try
            {
                data.Status = await _taskRepository.DeleteUseFullLink(requestModel.GuidId);

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while deleting the useFullLink.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        #endregion UseFullLinkMethods


        #region TaskCommentMethods
        public async Task<ApiResult<GetAllTaskCommentResponseModel>> GetAllTaskComment(GetAllTaskCommentRequestModel requestModel)
        {
            ApiResult<GetAllTaskCommentResponseModel> responseModel = new ApiResult<GetAllTaskCommentResponseModel>();
            GetAllTaskCommentResponseModel data = new GetAllTaskCommentResponseModel();

            List<TaskCommentModel> TaskCommentModels = new List<TaskCommentModel>();
            try
            {
                List<TaskComment> TaskComments = await _taskRepository.GetAllTaskCommentByTaskUniqueId(requestModel.TaskUniqueId);

                foreach (TaskComment TaskComment in TaskComments)
                {
                    TaskCommentModel TaskCommentModel = new TaskCommentModel();
                    TaskCommentModel.GuidId = TaskComment.GuidId;
                    TaskCommentModel.FirstName = TaskComment.FirstName;
                    TaskCommentModel.LastName = TaskComment.LastName;
                    TaskCommentModel.Comment = TaskComment.Comment;
                    TaskCommentModel.CommentedOn = TaskComment.CommentedOn;
                    TaskCommentModel.IsPreviouslyEdited = TaskComment.IsPreviouslyEdited;
                    TaskCommentModel.CommentEditedOn = TaskComment.CommentEditedOn;

                    string userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                    if (!string.IsNullOrEmpty(userId) && TaskComment.UserId == long.Parse(userId))
                        TaskCommentModel.isSameUserComment = true;
                    else
                        TaskCommentModel.isSameUserComment = false;

                    TaskCommentModels.Add(TaskCommentModel);
                }
                data.TaskComments = TaskCommentModels;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong. Unable to load the TaskComments";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<AddTaskCommentResponseModel>> AddTaskComment(AddTaskCommentRequestModel requestModel)
        {
            ApiResult<AddTaskCommentResponseModel> responseModel = new ApiResult<AddTaskCommentResponseModel>();
            AddTaskCommentResponseModel data = new AddTaskCommentResponseModel();

            try
            {
                TaskComment TaskComment = new TaskComment();

                TaskComment.TaskUniqueId = requestModel.TaskUniqueId;
                TaskComment.ProjectKey = requestModel.ProjectKey;
                TaskComment.FirstName = _httpContextAccessor.HttpContext?.User?.FindFirst("FirstName")?.Value;
                TaskComment.LastName = _httpContextAccessor.HttpContext?.User?.FindFirst("LastName")?.Value;
                TaskComment.Comment = requestModel.Comment;

                string userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                    TaskComment.UserId = long.Parse(userId);


                TaskComment = await _taskRepository.AddTaskComment(TaskComment);

                data.GuidId = TaskComment.GuidId;
                data.FirstName = TaskComment.FirstName;
                data.LastName = TaskComment.LastName;
                data.CommentedOn = TaskComment.CommentedOn;

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while adding the TaskComment.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<UpdateTaskCommentResponseModel>> UpdateTaskComment(UpdateTaskCommentRequestModel requestModel)
        {
            ApiResult<UpdateTaskCommentResponseModel> responseModel = new ApiResult<UpdateTaskCommentResponseModel>();
            UpdateTaskCommentResponseModel data = new UpdateTaskCommentResponseModel();

            try
            {
                TaskComment TaskComment = new TaskComment();

                TaskComment.GuidId = requestModel.GuidId;
                TaskComment.Comment = requestModel.Comment;

                TaskComment.IsPreviouslyEdited = true;
                TaskComment.CommentEditedOn = DateTimeOffset.Now;
                string userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                    TaskComment.UserId = long.Parse(userId);

                TaskComment = await _taskRepository.UpdateTaskComment(TaskComment);

                if(TaskComment == null)
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "You are not authorized to update this comment.";
                    responseModel.Status = false;
                    return responseModel;
                }

                else
                {
                    data.IsPreviouslyEdited = TaskComment.IsPreviouslyEdited;
                    data.CommentEditedOn = TaskComment.CommentEditedOn;

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while updating the TaskComment.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<DeleteTaskCommentResponseModel>> DeleteTaskComment(DeleteTaskCommentRequestModel requestModel)
        {
            ApiResult<DeleteTaskCommentResponseModel> responseModel = new ApiResult<DeleteTaskCommentResponseModel>();
            DeleteTaskCommentResponseModel data = new DeleteTaskCommentResponseModel();

            try
            {
                data.Status = await _taskRepository.DeleteTaskComment(requestModel.GuidId);

                if (!data.Status)
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "You are not authorized to delete this comment.";
                    responseModel.Status = false;
                    return responseModel;
                }

                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong while deleting the TaskComment.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        #endregion TaskCommentMethods


    }
}