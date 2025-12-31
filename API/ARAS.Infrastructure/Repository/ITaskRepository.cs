using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Domain.Models.Task;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Infrastructure.Services
{
    public interface ITaskRepository
    {
        public Task<List<CurrentTaskDTO>> GetAllCurrentTasks(string projectKey);
        public Task<List<TodayWorkCurrentTasks>> GetTodayWorkCurrentTasks(string projectKey, DateOnly displayDate);
        public Task<List<DoneTaskDTO>> GetAllDoneTasks(string projectKey);
        public Task<DailyTask> AddTask(DailyTask request);
        public Task<bool> MiniUpdateTask(DailyTask dailyTask);
        public Task<GetCurrentTaskByIdResponse> GetCurrentTaskById(Guid taskUniqueId);
        public Task<List<TodayWorkCurrentTasks>> GetTodayWorkCurrentTaskById(Guid taskUniqueId, DateOnly displayDate);
        public Task<DailyTask> UpdateTask(DailyTask dailyTask);
        public Task<bool> MoveDailyTask(Guid taskUniqueId, bool isAlive);

        public Task<List<SubTask>> GetSubTasksById(Guid taskUniqueId);
        public Task<List<SubTaskWorkEntry>> GetSubTaskWorkEntryById(Guid taskUniqueId);
        public Task<decimal> AddSubTask(SubTask subTask);
        public Task<decimal> UpdateSubTask(SubTask subTask);
        public Task<AddSubTaskWorkEntryResponse> AddSubTaskWorkEntry(SubTaskWorkEntry subTaskWorkEntry);
        public Task<UpdateSubTaskWorkEntryResponse> UpdateSubTaskWorkEntry(UpdateSubTaskWorkEntryRequest request);
        public Task<ApproveETAResponse> ApproveETA(ApproveETARequest request);
        public Task<List<ShowTaskToCopyDTO>> GetTasksToCopy(string projectKey, bool isAlive);
        public Task<List<ShowSubTaskToCopyDTO>> GetSubTasksToCopy(Guid taskUniqueId);


        public Task<List<UseFullLink>> GetAllUseFullLinkByProjectKey(string projectKey);
        public Task<bool> DeleteUseFullLink(Guid guidId);
        public Task<UseFullLink> AddUseFullLink(UseFullLink useFullLink);
        public Task<UseFullLink> UpdateUseFullLink(UseFullLink useFullLink);

        public Task<List<TaskComment>> GetAllTaskCommentByTaskUniqueId(Guid taskUniqueId);
        public Task<bool> DeleteTaskComment(Guid guidId);
        public Task<TaskComment> AddTaskComment(TaskComment TaskComment);
        public Task<TaskComment> UpdateTaskComment(TaskComment TaskComment);
        public Task<List<DropDownValueModel>> RNAndFeatureListByProjectKey(string projectKey);
        public Task<RNAndFeature> AddRNAndFeature(RNAndFeature rNAndFeature);
        public Task<bool> UpdateRNAndFeature(Guid guidId, string rN);
        public Task<List<TaskForRN>> GetAllTasksByRNGuidId(string projectKey, Guid guidId);
    }
}
