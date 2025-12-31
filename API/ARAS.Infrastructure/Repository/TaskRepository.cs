using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ARAS.Domain.Models.Task;
using ARAS.Infrastructure.DBContext;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ARAS.Infrastructure.Services
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;
        private readonly ARASDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskRepository(ARASDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        private long GetUserId()
        {
            long userId = 0;
            try
            {
                string userIdS = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                userId = long.Parse(userIdS);
            }
            catch
            {
                userId = 0;
            }
            return userId;
        }

        public async Task<List<CurrentTaskDTO>> GetAllCurrentTasks(string projectKey)
        {
            List<CurrentTaskDTO> response = new List<CurrentTaskDTO>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetAllCurrentTasks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProjectKey", projectKey);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordUserId = reader.GetOrdinal("UserId");
                var ordUserName = reader.GetOrdinal("UserName");
                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordTaskSeq = reader.GetOrdinal("TaskSeq");
                var ordCatagory = reader.GetOrdinal("Catagory");
                var ordTaskName = reader.GetOrdinal("TaskName");
                var ordSubProject = reader.GetOrdinal("SubProject");
                var ordNetwork = reader.GetOrdinal("Network");
                var ordStatus = reader.GetOrdinal("Status");
                var ordTodayDayWork = reader.GetOrdinal("TodayDayWork");
                var ordItemToDiscuss = reader.GetOrdinal("ItemToDiscuss");
                var ordEHrsToday = reader.GetOrdinal("EHrsToday");
                var ordMyComments = reader.GetOrdinal("MyComments");
                var ordManagerComments = reader.GetOrdinal("ManagerComments");
                var ordJira = reader.GetOrdinal("Jira");
                var ordTotalETA = reader.GetOrdinal("TotalETA");
                var ordUsedETA = reader.GetOrdinal("UsedETA");
                var ordOtherUsedETA = reader.GetOrdinal("OtherUsedETA");
                var ordIsApprove = reader.GetOrdinal("IsApprove");
                var ordRNGuidId = reader.GetOrdinal("RNGuidId");
                var ordFixVersion = reader.GetOrdinal("FixVersion");
                var ordMailTitled = reader.GetOrdinal("MailTitled");

                while (await reader.ReadAsync())
                {
                    CurrentTaskDTO data = new CurrentTaskDTO();

                    data.UserId = reader.GetInt64(ordUserId);
                    data.UserName = reader.GetString(ordUserName);
                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.TaskSeq = reader.GetInt32(ordTaskSeq);
                    data.Catagory = reader.GetString(ordCatagory);
                    data.TaskName = reader.GetString(ordTaskName);
                    data.SubProject = reader.GetString(ordSubProject);
                    data.Network = reader.GetString(ordNetwork);
                    data.Status = reader.GetString(ordStatus);
                    data.TodayDayWork = reader.GetBoolean(ordTodayDayWork);
                    data.ItemToDiscuss = reader.GetBoolean(ordItemToDiscuss);
                    data.EHrsToday = reader.GetString(ordEHrsToday);
                    data.MyComments = reader.GetString(ordMyComments);
                    data.ManagerComments = reader.GetString(ordManagerComments);
                    data.Jira = reader.GetString(ordJira);
                    data.TotalETA = reader.GetDecimal(ordTotalETA);
                    data.UsedETA = reader.GetDecimal(ordUsedETA);
                    data.OtherUsedETA = reader.GetDecimal(ordOtherUsedETA);
                    data.IsApprove = reader.GetBoolean(ordIsApprove);
                    data.RNGuidId = reader.GetGuid(ordRNGuidId);
                    data.FixVersion = reader.GetString(ordFixVersion);
                    data.MailTitled = reader.GetString(ordMailTitled);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }

        public async Task<List<TodayWorkCurrentTasks>> GetTodayWorkCurrentTasks(string projectKey, DateOnly displayDate)
        {
            List<TodayWorkCurrentTasks> response = new List<TodayWorkCurrentTasks>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetTodayWorkCurrentTasks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProjectKey", projectKey);
                command.Parameters.Add("@DisplayDate", SqlDbType.Date).Value = displayDate.ToDateTime(TimeOnly.MinValue);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordWorkHrs = reader.GetOrdinal("WorkHrs");
                var ordFirstName = reader.GetOrdinal("FirstName");

                while (await reader.ReadAsync())
                {
                    TodayWorkCurrentTasks data = new TodayWorkCurrentTasks();

                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.FirstName = reader.GetString(ordFirstName);
                    data.WorksHrs = reader.GetDecimal(ordWorkHrs);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }
            return response;
        }

        public async Task<List<DoneTaskDTO>> GetAllDoneTasks(string projectKey)
        {
            List<DoneTaskDTO> response = new List<DoneTaskDTO>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetAllDoneTasks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProjectKey", projectKey);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordUserId = reader.GetOrdinal("UserId");
                var ordUserName = reader.GetOrdinal("UserName");
                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordTaskSeq = reader.GetOrdinal("TaskSeq");
                var ordCatagory = reader.GetOrdinal("Catagory");
                var ordTaskName = reader.GetOrdinal("TaskName");
                var ordSubProject = reader.GetOrdinal("SubProject");
                var ordNetwork = reader.GetOrdinal("Network");
                var ordStatus = reader.GetOrdinal("Status");
                var ordMyComments = reader.GetOrdinal("MyComments");
                var ordManagerComments = reader.GetOrdinal("ManagerComments");
                var ordJira = reader.GetOrdinal("Jira");
                var ordTotalETA = reader.GetOrdinal("TotalETA");
                var ordUsedETA = reader.GetOrdinal("UsedETA");
                var ordOtherUsedETA = reader.GetOrdinal("OtherUsedETA");
                var ordRNName = reader.GetOrdinal("RNName");
                var ordFixVersion = reader.GetOrdinal("FixVersion");
                var ordMailTitled = reader.GetOrdinal("MailTitled");

                while (await reader.ReadAsync())
                {
                    DoneTaskDTO data = new DoneTaskDTO();

                    data.UserId = reader.GetInt64(ordUserId);
                    data.UserName = reader.GetString(ordUserName);
                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.TaskSeq = reader.GetInt32(ordTaskSeq);
                    data.Catagory = reader.GetString(ordCatagory);
                    data.TaskName = reader.GetString(ordTaskName);
                    data.SubProject = reader.GetString(ordSubProject);
                    data.Network = reader.GetString(ordNetwork);
                    data.Status = reader.GetString(ordStatus);
                    data.MyComments = reader.GetString(ordMyComments);
                    data.ManagerComments = reader.GetString(ordManagerComments);
                    data.Jira = reader.GetString(ordJira);
                    data.TotalETA = reader.GetDecimal(ordTotalETA);
                    data.UsedETA = reader.GetDecimal(ordUsedETA);
                    data.OtherUsedETA = reader.GetDecimal(ordOtherUsedETA);
                    data.RNName = reader.GetString(ordRNName);
                    data.FixVersion = reader.GetString(ordFixVersion);
                    data.MailTitled = reader.GetString(ordMailTitled);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }
            return response;
        }

        public async Task<DailyTask> AddTask(DailyTask dailyTask)
        {
            dailyTask.TaskSeq = await _dbContext.DailyTasks.Where(u => u.ProjectKey == dailyTask.ProjectKey).MaxAsync(u => (int?)u.TaskSeq) ?? 0;
            dailyTask.TaskSeq += 1;

            await _dbContext.DailyTasks.AddAsync(dailyTask);

            DailyTasksLog dailyTasksLog = new DailyTasksLog();

            dailyTasksLog.TaskId = dailyTask.TaskUniqueId;
            dailyTasksLog.Log = "New task added";
            dailyTasksLog.UserId = GetUserId();

            await _dbContext.DailyTasksLogs.AddAsync(dailyTasksLog);
            await _dbContext.SaveChangesAsync();

            return dailyTask;
        }



        public async Task<bool> MiniUpdateTask(DailyTask dailyTask)
        {
            var entry = await _dbContext.DailyTasks.FirstOrDefaultAsync(u => u.TaskUniqueId == dailyTask.TaskUniqueId);

            if (entry != null)
            {
                long userId = GetUserId();

                UserRole userRole =  await _dbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId && (u.RoleId == 101 || u.RoleId == 102));

                var changeLog = new StringBuilder();

                void LogChange<T>(string propName, T oldValue, T newValue, Action updateAction)
                {
                    if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
                    {
                        changeLog.AppendLine($"{propName} From \"{oldValue}\" To \"{newValue}\"");
                        updateAction();
                    }
                }

                #region Check changes
                LogChange(nameof(entry.ItemToDiscuss), entry.ItemToDiscuss, dailyTask.ItemToDiscuss, () => entry.ItemToDiscuss = dailyTask.ItemToDiscuss);

                if(userRole != null)
                {
                    LogChange(nameof(entry.EHrsToday), entry.EHrsToday, dailyTask.EHrsToday, () => entry.EHrsToday = dailyTask.EHrsToday);
                    LogChange(nameof(entry.TodayDayWork), entry.TodayDayWork, dailyTask.TodayDayWork, () => entry.TodayDayWork = dailyTask.TodayDayWork);
                    LogChange(nameof(entry.ManagerComments), entry.ManagerComments, dailyTask.ManagerComments, () => entry.ManagerComments = dailyTask.ManagerComments);
                }

                #endregion Check changes

                var logOutput = changeLog.ToString();
                if (!string.IsNullOrWhiteSpace(logOutput))
                {
                    DailyTasksLog dailyTasksLog = new DailyTasksLog();

                    dailyTasksLog.TaskId = dailyTask.TaskUniqueId;
                    dailyTasksLog.Log = logOutput;
                    dailyTasksLog.UserId = userId;

                    await _dbContext.DailyTasksLogs.AddAsync(dailyTasksLog);
                }
            }
            else
            {
                await _dbContext.DailyTasks.AddAsync(dailyTask);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }


        public async Task<GetCurrentTaskByIdResponse> GetCurrentTaskById(Guid taskUniqueId)
        {
            GetCurrentTaskByIdResponse response = new GetCurrentTaskByIdResponse();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetCurrentTaskById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TaskUniqueId", taskUniqueId);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordUserId = reader.GetOrdinal("UserId");
                var ordUserName = reader.GetOrdinal("UserName");
                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordTaskSeq = reader.GetOrdinal("TaskSeq");
                var ordCatagory = reader.GetOrdinal("Catagory");
                var ordTaskName = reader.GetOrdinal("TaskName");
                var ordSubProject = reader.GetOrdinal("SubProject");
                var ordNetwork = reader.GetOrdinal("Network");
                var ordStatus = reader.GetOrdinal("Status");
                var ordTodayDayWork = reader.GetOrdinal("TodayDayWork");
                var ordItemToDiscuss = reader.GetOrdinal("ItemToDiscuss");
                var ordEHrsToday = reader.GetOrdinal("EHrsToday");
                var ordMyComments = reader.GetOrdinal("MyComments");
                var ordManagerComments = reader.GetOrdinal("ManagerComments");
                var ordJira = reader.GetOrdinal("Jira");
                var ordTotalETA = reader.GetOrdinal("TotalETA");
                var ordUsedETA = reader.GetOrdinal("UsedETA");
                var ordOtherUsedETA = reader.GetOrdinal("OtherUsedETA");
                var ordRNGuidId = reader.GetOrdinal("RNGuidId");
                var ordFeatureName = reader.GetOrdinal("FeatureName");
                var ordFixVersion = reader.GetOrdinal("FixVersion");
                var ordRNComments = reader.GetOrdinal("RNComments");
                var ordMailTitled = reader.GetOrdinal("MailTitled");

                while (await reader.ReadAsync())
                {
                    response.UserId = reader.GetInt64(ordUserId);
                    response.UserName = reader.GetString(ordUserName);
                    response.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    response.TaskSeq = reader.GetInt32(ordTaskSeq);
                    response.Catagory = reader.GetString(ordCatagory);
                    response.TaskName = reader.GetString(ordTaskName);
                    response.SubProject = reader.GetString(ordSubProject);
                    response.Network = reader.GetString(ordNetwork);
                    response.Status = reader.GetString(ordStatus);
                    response.TodayDayWork = reader.GetBoolean(ordTodayDayWork);
                    response.ItemToDiscuss = reader.GetBoolean(ordItemToDiscuss);
                    response.EHrsToday = reader.GetString(ordEHrsToday);
                    response.MyComments = reader.GetString(ordMyComments);
                    response.ManagerComments = reader.GetString(ordManagerComments);
                    response.Jira = reader.GetString(ordJira);
                    response.TotalETA = reader.GetDecimal(ordTotalETA);
                    response.UsedETA = reader.GetDecimal(ordUsedETA);
                    response.OtherUsedETA = reader.GetDecimal(ordOtherUsedETA);
                    response.IsApprove = reader.GetBoolean(reader.GetOrdinal("IsApprove"));
                    response.RNGuidId = reader.GetGuid(ordRNGuidId);
                    response.FeatureName = reader.GetString(ordFeatureName);
                    response.FixVersion = reader.GetString(ordFixVersion);
                    response.RNComments = reader.GetString(ordRNComments);
                    response.MailTitled = reader.GetString(ordMailTitled);
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }

        public async Task<List<TodayWorkCurrentTasks>> GetTodayWorkCurrentTaskById(Guid taskUniqueId, DateOnly displayDate)
        {
            List<TodayWorkCurrentTasks> response = new List<TodayWorkCurrentTasks>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetTodayWorkCurrentTaskById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@TaskUniqueId", SqlDbType.UniqueIdentifier).Value = taskUniqueId;
                command.Parameters.Add("@DisplayDate", SqlDbType.Date).Value = displayDate.ToDateTime(TimeOnly.MinValue);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordWorkHrs = reader.GetOrdinal("WorkHrs");
                var ordFirstName = reader.GetOrdinal("FirstName");

                while (await reader.ReadAsync())
                {
                    TodayWorkCurrentTasks data = new TodayWorkCurrentTasks();

                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.FirstName = reader.GetString(ordFirstName);
                    data.WorksHrs = reader.GetDecimal(ordWorkHrs);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }
            return response;
        }
        public async Task<DailyTask> UpdateTask(DailyTask dailyTask)
        {
            var entry = await _dbContext.DailyTasks.FirstOrDefaultAsync(u => u.TaskUniqueId == dailyTask.TaskUniqueId);

            if (entry != null)
            {
                var changeLog = new StringBuilder();

                void LogChange<T>(string propName, T oldValue, T newValue, Action updateAction)
                {
                    if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
                    {
                        changeLog.AppendLine($"{propName} From \"{oldValue}\" To \"{newValue}\"");
                        updateAction();
                    }
                }

                #region Check changes

                if (entry.UserName != dailyTask.UserName)
                {
                    if (!entry.IsApprove)
                    {
                        var subTaskWorkEntry = await _dbContext.SubTaskWorkEntrys.FirstOrDefaultAsync(u => u.TaskUniqueId == entry.TaskUniqueId && u.UserId != entry.UserId);
                        if (subTaskWorkEntry == null)
                        {
                            changeLog.AppendLine($"UserName From \"{entry.UserName}\" To \"{dailyTask.UserName}\"");
                            entry.UserName = dailyTask.UserName;
                            entry.UserId = dailyTask.UserId;
                        }
                        
                    }
                }


                LogChange(nameof(entry.Catagory), entry.Catagory, dailyTask.Catagory, () => entry.Catagory = dailyTask.Catagory);
                LogChange(nameof(entry.TaskName), entry.TaskName, dailyTask.TaskName, () => entry.TaskName = dailyTask.TaskName);
                LogChange(nameof(entry.SubProject), entry.SubProject, dailyTask.SubProject, () => entry.SubProject = dailyTask.SubProject);
                LogChange(nameof(entry.Network), entry.Network, dailyTask.Network, () => entry.Network = dailyTask.Network);
                LogChange(nameof(entry.Status), entry.Status, dailyTask.Status, () => entry.Status = dailyTask.Status);
                LogChange(nameof(entry.ItemToDiscuss), entry.ItemToDiscuss, dailyTask.ItemToDiscuss, () => entry.ItemToDiscuss = dailyTask.ItemToDiscuss);
                LogChange(nameof(entry.MyComments), entry.MyComments, dailyTask.MyComments, () => entry.MyComments = dailyTask.MyComments);

                LogChange(nameof(entry.Jira), entry.Jira, dailyTask.Jira, () => entry.Jira = dailyTask.Jira);

                LogChange(nameof(entry.FeatureName), entry.FeatureName, dailyTask.FeatureName, () => entry.FeatureName = dailyTask.FeatureName);
                LogChange(nameof(entry.FixVersion), entry.FixVersion, dailyTask.FixVersion, () => entry.FixVersion = dailyTask.FixVersion);
                LogChange(nameof(entry.RNComments), entry.RNComments, dailyTask.RNComments, () => entry.RNComments = dailyTask.RNComments);
                LogChange(nameof(entry.MailTitled), entry.MailTitled, dailyTask.MailTitled, () => entry.MailTitled = dailyTask.MailTitled);
                if (entry.RNGuidId != dailyTask.RNGuidId)
                {
                    entry.RNGuidId = dailyTask.RNGuidId;
                }

                    #endregion Check changes

                    var logOutput = changeLog.ToString();
                if (!string.IsNullOrWhiteSpace(logOutput))
                {
                    DailyTasksLog dailyTasksLog = new DailyTasksLog();

                    dailyTasksLog.TaskId = dailyTask.TaskUniqueId;
                    dailyTasksLog.Log = logOutput;
                    dailyTasksLog.UserId = GetUserId();

                    await _dbContext.DailyTasksLogs.AddAsync(dailyTasksLog);
                }
            }
            else
            {
                await _dbContext.DailyTasks.AddAsync(dailyTask);
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return dailyTask;
        }


        public async Task<bool> MoveDailyTask(Guid taskUniqueId, bool isAlive)
        {
            if (taskUniqueId != null)
            {
                var entry = await _dbContext.DailyTasks.FirstOrDefaultAsync(u => u.TaskUniqueId == taskUniqueId);

                if (entry != null)
                {
                    entry.IsAlive = isAlive;

                    DailyTasksLog dailyTasksLog = new DailyTasksLog();

                    dailyTasksLog.TaskId = taskUniqueId;
                    dailyTasksLog.UserId = GetUserId();
                    if (isAlive)
                    {
                        dailyTasksLog.Log = "Task unmove from Done to Current";
                    }
                    else
                    {
                        dailyTasksLog.Log = "Task move from Current to Done";
                    }
                    await _dbContext.DailyTasksLogs.AddAsync(dailyTasksLog);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<List<SubTask>> GetSubTasksById(Guid taskUniqueId)
        {
            List<SubTask> subTasks = await _dbContext.SubTasks.Where(u => u.TaskUniqueId == taskUniqueId).OrderBy(u => u.SubTaskSeq).ToListAsync();
            return subTasks;
        }

        public async Task<decimal> AddSubTask(SubTask subTask)
        {
            subTask.UserId = GetUserId();

            decimal totalETA = 0;

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("AddSubTask", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CurrentUser_UserId", SqlDbType.BigInt).Value = subTask.UserId;
                command.Parameters.Add("@SubTaskUniqueId", SqlDbType.UniqueIdentifier).Value = subTask.SubTaskUniqueId;
                command.Parameters.Add("@TaskUniqueId", SqlDbType.UniqueIdentifier).Value = subTask.TaskUniqueId;
                command.Parameters.Add("@SubTaskSeq", SqlDbType.Int).Value = subTask.SubTaskSeq;
                command.Parameters.AddWithValue("@SubTaskName", subTask.SubTaskName);
                command.Parameters.AddWithValue("@Remark", subTask.Remark);
                command.Parameters.Add("@SubTaskETA", SqlDbType.Decimal).Value = subTask.SubTaskETA;
                command.Parameters.AddWithValue("@Status", subTask.Status);
                command.Parameters.Add("@IsColour", SqlDbType.Bit).Value = subTask.IsColour;
                command.Parameters.Add("@IsSelf", SqlDbType.Bit).Value = subTask.IsSelf;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                while (await reader.ReadAsync())
                {
                    totalETA = reader.GetDecimal(reader.GetOrdinal("TotalETA"));
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return totalETA;
        }

        public async Task<decimal> UpdateSubTask(SubTask subTask)
        {
            subTask.UserId = GetUserId();

            decimal totalETA = 0;

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("UpdateSubTask", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CurrentUser_UserId", SqlDbType.BigInt).Value = subTask.UserId;
                command.Parameters.Add("@SubTaskUniqueId", SqlDbType.UniqueIdentifier).Value = subTask.SubTaskUniqueId;
                command.Parameters.Add("@SubTaskSeq", SqlDbType.Int).Value = subTask.SubTaskSeq;
                command.Parameters.AddWithValue("@SubTaskName", subTask.SubTaskName);
                command.Parameters.AddWithValue("@Remark", subTask.Remark);
                command.Parameters.Add("@SubTaskETA", SqlDbType.Decimal).Value = subTask.SubTaskETA;
                command.Parameters.AddWithValue("@Status", subTask.Status);
                command.Parameters.Add("@IsColour", SqlDbType.Bit).Value = subTask.IsColour;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                while (await reader.ReadAsync())
                {
                    totalETA = reader.GetDecimal(reader.GetOrdinal("TotalETA"));
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return totalETA;
        }
        public async Task<List<SubTaskWorkEntry>> GetSubTaskWorkEntryById(Guid taskUniqueId)
        {
            List<SubTaskWorkEntry> subTaskWorkEntrys = await _dbContext.SubTaskWorkEntrys.Where(u => u.TaskUniqueId == taskUniqueId).ToListAsync();

            return subTaskWorkEntrys;
        }
        public async Task<AddSubTaskWorkEntryResponse> AddSubTaskWorkEntry(SubTaskWorkEntry subTaskWorkEntry)
        {
            AddSubTaskWorkEntryResponse response = new AddSubTaskWorkEntryResponse();

            response.UserId = GetUserId();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("AddSubTaskWorkEntry", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CurrentUser_UserId", SqlDbType.BigInt).Value = response.UserId;
                command.Parameters.Add("@SubTaskUniqueId", SqlDbType.UniqueIdentifier).Value = subTaskWorkEntry.SubTaskUniqueId;
                command.Parameters.Add("@TaskUniqueId", SqlDbType.UniqueIdentifier).Value = subTaskWorkEntry.TaskUniqueId;
                command.Parameters.Add("@EntryDate", SqlDbType.Date).Value = subTaskWorkEntry.EntryDate;
                command.Parameters.Add("@EntryHrs", SqlDbType.Decimal).Value = subTaskWorkEntry.EntryHrs;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                while (await reader.ReadAsync())
                {
                    response.IsError = reader.GetBoolean(reader.GetOrdinal("IsError"));
                    response.ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage"));
                    response.WorkEntryUniqueId = reader.GetGuid(reader.GetOrdinal("WorkEntryUniqueId"));
                    response.EntryHrs = reader.GetDecimal(reader.GetOrdinal("EntryHrs"));
                    response.UsedETA = reader.GetDecimal(reader.GetOrdinal("UsedETA"));
                    response.OtherUsedETA = reader.GetDecimal(reader.GetOrdinal("OtherUsedETA"));
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }
        public async Task<UpdateSubTaskWorkEntryResponse> UpdateSubTaskWorkEntry(UpdateSubTaskWorkEntryRequest request)
        {
            UpdateSubTaskWorkEntryResponse response = new UpdateSubTaskWorkEntryResponse();

            long userId = GetUserId();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("UpdateSubTaskWorkEntry", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CurrentUser_UserId", SqlDbType.BigInt).Value = userId;
                command.Parameters.Add("@WorkEntryUniqueId", SqlDbType.UniqueIdentifier).Value = request.WorkEntryUniqueId;
                command.Parameters.Add("@EntryHrs", SqlDbType.Decimal).Value = request.EntryHrs;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                while (await reader.ReadAsync())
                {
                    response.IsError = reader.GetBoolean(reader.GetOrdinal("IsError"));
                    response.EntryHrs = reader.GetDecimal(reader.GetOrdinal("EntryHrs"));
                    response.UsedETA = reader.GetDecimal(reader.GetOrdinal("UsedETA"));
                    response.OtherUsedETA = reader.GetDecimal(reader.GetOrdinal("OtherUsedETA"));
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }


        public async Task<ApproveETAResponse> ApproveETA(ApproveETARequest request)
        {
            ApproveETAResponse response = new ApproveETAResponse();

            long userId = GetUserId();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("ApproveETA", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@CurrentUser_UserId", SqlDbType.BigInt).Value = userId;
                command.Parameters.Add("@UniqueId", SqlDbType.UniqueIdentifier).Value = request.UniqueId;
                command.Parameters.Add("@IsAll", SqlDbType.Bit).Value = request.IsAll;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                while (await reader.ReadAsync())
                {
                    response.IsError = reader.GetBoolean(reader.GetOrdinal("IsError"));
                    response.ErrorMessage = reader.GetString(reader.GetOrdinal("ErrorMessage"));
                    response.TotalETA = reader.GetDecimal(reader.GetOrdinal("TotalETA"));
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }
        public async Task<List<ShowTaskToCopyDTO>> GetTasksToCopy(string projectKey, bool isAlive)
        {
            List<ShowTaskToCopyDTO> response = new List<ShowTaskToCopyDTO>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetTasksToCopy", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProjectKey", projectKey);
                command.Parameters.Add("@IsAlive", SqlDbType.Bit).Value = isAlive;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordFirstName = reader.GetOrdinal("FirstName");
                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordTaskSeq = reader.GetOrdinal("TaskSeq");
                var ordTaskName = reader.GetOrdinal("TaskName");
                var ordSubProject = reader.GetOrdinal("SubProject");
                var ordNetwork = reader.GetOrdinal("Network");
                var ordStatus = reader.GetOrdinal("Status");

                while (await reader.ReadAsync())
                {
                    ShowTaskToCopyDTO data = new ShowTaskToCopyDTO();

                    data.FirstName = reader.GetString(ordFirstName);
                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.TaskSeq = reader.GetInt32(ordTaskSeq);
                    data.TaskName = reader.GetString(ordTaskName);
                    data.SubProject = reader.GetString(ordSubProject);
                    data.Network = reader.GetString(ordNetwork);
                    data.Status = reader.GetString(ordStatus);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }

        public async Task<List<ShowSubTaskToCopyDTO>> GetSubTasksToCopy(Guid taskUniqueId)
        {
            List<ShowSubTaskToCopyDTO> response = new List<ShowSubTaskToCopyDTO>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetSubTasksToCopy", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@TaskUniqueId", SqlDbType.UniqueIdentifier).Value = taskUniqueId;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordSubTaskSeq = reader.GetOrdinal("SubTaskSeq");
                var ordSubTaskName = reader.GetOrdinal("SubTaskName");
                var ordSubTaskETA = reader.GetOrdinal("SubTaskETA");

                while (await reader.ReadAsync())
                {
                    ShowSubTaskToCopyDTO data = new ShowSubTaskToCopyDTO();

                    data.SubTaskSeq = reader.GetInt32(ordSubTaskSeq);
                    data.SubTaskName = reader.GetString(ordSubTaskName);
                    data.SubTaskETA = reader.GetDecimal(ordSubTaskETA);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }

        public async Task<List<DropDownValueModel>> RNAndFeatureListByProjectKey(string projectKey)
        {
            return await _dbContext.RNAndFeatureList.Where(u => u.ProjectKey == projectKey).OrderByDescending(a => a.Id)
         .Select(u => new DropDownValueModel
         {
             GuidId = u.GuidId,
             Value = u.RN
         })
         .ToListAsync();
        }

        public async Task<RNAndFeature> AddRNAndFeature(RNAndFeature rNAndFeature)
        {
            rNAndFeature.AddedByUserId = GetUserId();
            await _dbContext.RNAndFeatureList.AddAsync(rNAndFeature);
            await _dbContext.SaveChangesAsync();

            return rNAndFeature;
        }
        public async Task<bool> UpdateRNAndFeature(Guid guidId, string rN)
        {
            var entry = await _dbContext.RNAndFeatureList
                .FirstOrDefaultAsync(u => u.GuidId == guidId);

            if (entry != null)
            {
                #region Check changes
                if (entry.RN != rN)
                {
                    entry.RN = rN;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<TaskForRN>> GetAllTasksByRNGuidId(string projectKey, Guid guidId)
        {
            List<TaskForRN> response = new List<TaskForRN>();

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand("GetAllTasksByRNGuidId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ProjectKey", projectKey);
                command.Parameters.Add("@GuidId", SqlDbType.UniqueIdentifier).Value = guidId;

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                var ordTaskUniqueId = reader.GetOrdinal("TaskUniqueId");
                var ordJira = reader.GetOrdinal("Jira");
                var ordFeatureName = reader.GetOrdinal("FeatureName");
                var ordFixVersion = reader.GetOrdinal("FixVersion");
                var ordRNComments = reader.GetOrdinal("RNComments");
                var ordFirstName = reader.GetOrdinal("FirstName");
                var ordIsAlive = reader.GetOrdinal("IsAlive");

                while (await reader.ReadAsync())
                {
                    TaskForRN data = new TaskForRN();
         
                    data.TaskUniqueId = reader.GetGuid(ordTaskUniqueId);
                    data.Jira = reader.GetString(ordJira);
                    data.FeatureName = reader.GetString(ordFeatureName);
                    data.FixVersion = reader.GetString(ordFixVersion);
                    data.RNComments = reader.GetString(ordRNComments);
                    data.FirstName = reader.GetString(ordFirstName);
                    data.IsAlive = reader.GetBoolean(ordIsAlive);

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($" Error: {ex.Message}");
                throw;
            }

            return response;
        }

        //    public async Task<AddTaskResponse> AddTask(AddTaskRequest request)
        //    {
        //        AddTaskResponse response = new AddTaskResponse();

        //        try
        //        {
        //            await using var connection = new SqlConnection(_connectionString);
        //            await using var command = new SqlCommand("AddCurrentTask", connection)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };
        //            command.Parameters.AddWithValue("@UserName", request.UserName);
        //            command.Parameters.Add("@TaskUniqueId", SqlDbType.UniqueIdentifier).Value = response.TaskUniqueId;
        //            command.Parameters.AddWithValue("@Catagory", request.Catagory);
        //            command.Parameters.AddWithValue("@TaskName", request.TaskName);
        //            command.Parameters.AddWithValue("@SubProject", request.SubProject);
        //            command.Parameters.AddWithValue("@ProjectKey", request.ProjectKey);
        //            command.Parameters.Add("@ProjectId", SqlDbType.Int).Value = response.ProjectId;

        //            command.Parameters.AddWithValue("@Network", request.Network);
        //            command.Parameters.AddWithValue("@Status", request.Status);
        //            command.Parameters.Add("@UserId", SqlDbType.BigInt).Value = response.UserId;

        //            command.Parameters.AddWithValue("@MyComments", request.MyComments);
        //            command.Parameters.AddWithValue("@Jira", request.Jira);
        //            command.Parameters.Add("@UserId", SqlDbType.BigInt).Value = response.UserId;









        //            @ UNIQUEIDENTIFIER,
        //            @       NVARCHAR(200),
        //            @ NVARCHAR(200),
        //            @ NVARCHAR(200),
        //            @ NVARCHAR(200),
        //            @ INT,
        //            @        NVARCHAR(200),
        //            @ NVARCHAR(200),
        //            @ItemToDiscuss BIT,
        //            @     NVARCHAR(MAX),
        //            @ NVARCHAR

        //            await connection.OpenAsync();
        //            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);



        //            while (await reader.ReadAsync())
        //            {
        //                var ordItemToDiscuss = reader.GetOrdinal("ItemToDiscuss");
        //                var ordEHrsToday = reader.GetOrdinal("EHrsToday");
        //                var ordMyComments = reader.GetOrdinal("MyComments");
        //                var ordManagerComments = reader.GetOrdinal("ManagerComments");
        //                var ordJira = reader.GetOrdinal("Jira");
        //                var ordTotalETA = reader.GetOrdinal("TotalETA");
        //                var ordUsedETA = reader.GetOrdinal("UsedETA");
        //                var ordOtherUsedETA = reader.GetOrdinal("OtherUsedETA");

        //                response.UserId = reader.GetInt64(reader.GetOrdinal("UserId"));
        //                response.UserName = reader.GetString(reader.GetOrdinal("UserName"));
        //                response.TaskUniqueId = reader.GetGuid(reader.GetOrdinal("TaskUniqueId"));
        //                response.TaskSeq = reader.GetInt32(reader.GetOrdinal("TaskSeq"));
        //                response.Catagory = reader.GetString(reader.GetOrdinal("Catagory"));
        //                response.TaskName = reader.GetString(reader.GetOrdinal("TaskName"));
        //                response.SubProject = reader.GetString(reader.GetOrdinal("SubProject"));
        //                response.Network = reader.GetString(reader.GetOrdinal("Network"));
        //                response.Status = reader.GetString(reader.GetOrdinal("Status"));
        //                response.TodayDayWork = reader.GetBoolean(reader.GetOrdinal("TodayDayWork"));
        //                response.ItemToDiscuss = reader.GetBoolean(ordItemToDiscuss);
        //                response.EHrsToday = reader.GetString(ordEHrsToday);
        //                response.MyComments = reader.GetString(ordMyComments);
        //                response.ManagerComments = reader.GetString(ordManagerComments);
        //                response.Jira = reader.GetString(ordJira);
        //                response.TotalETA = reader.GetDecimal(ordTotalETA);
        //                response.UsedETA = reader.GetDecimal(ordUsedETA);
        //                response.OtherUsedETA = reader.GetDecimal(ordOtherUsedETA);
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Error.WriteLine($" Error: {ex.Message}");
        //            throw;
        //        }

        //        return response;
        //    }



        #region UseFullLink

        public async Task<List<UseFullLink>> GetAllUseFullLinkByProjectKey(string projectKey)
        {
            List<UseFullLink> useFullLinks = await _dbContext.UseFullLinks.Where(u => u.ProjectKey == projectKey).ToListAsync();
            return useFullLinks;
        }

        public async Task<bool> DeleteUseFullLink(Guid guidId)
        {
            await _dbContext.UseFullLinks.Where(u => u.GuidId == guidId).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UseFullLink> AddUseFullLink(UseFullLink useFullLink)
        {
            await _dbContext.UseFullLinks.AddAsync(useFullLink);
            await _dbContext.SaveChangesAsync();

            return useFullLink;
        }
        public async Task<UseFullLink> UpdateUseFullLink(UseFullLink useFullLink)
        {
            var entry = await _dbContext.UseFullLinks
                .FirstOrDefaultAsync(u => u.GuidId == useFullLink.GuidId);
            if (entry != null)
            {
                #region Check changes
                if (entry.LinkName != useFullLink.LinkName)
                {
                    entry.LinkName = useFullLink.LinkName;
                }
                if (entry.LinkURL != useFullLink.LinkURL)
                {
                    entry.LinkURL = useFullLink.LinkURL;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }

            return useFullLink;
        }

        #endregion UseFullLink


        #region TaskComment

        public async Task<List<TaskComment>> GetAllTaskCommentByTaskUniqueId(Guid taskUniqueId)
        {
            List<TaskComment> TaskComments = await _dbContext.TaskComments.Where(u => u.TaskUniqueId == taskUniqueId).OrderByDescending(u => u.CommentId).ToListAsync();
            return TaskComments;
        }

        public async Task<bool> DeleteTaskComment(Guid guidId)
        {
            long userId = GetUserId();
            var entry = await _dbContext.TaskComments
                .FirstOrDefaultAsync(u => u.GuidId == guidId);

            if (entry != null)
            {
                if (entry.UserId != userId)
                {
                    return false;
                }
                await _dbContext.TaskComments.Where(u => u.GuidId == guidId).ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();

            }
            return true;
        }

        public async Task<TaskComment> AddTaskComment(TaskComment TaskComment)
        {
            await _dbContext.TaskComments.AddAsync(TaskComment);
            await _dbContext.SaveChangesAsync();

            return TaskComment;
        }
        public async Task<TaskComment> UpdateTaskComment(TaskComment TaskComment)
        {
            var entry = await _dbContext.TaskComments
                .FirstOrDefaultAsync(u => u.GuidId == TaskComment.GuidId);
            if (entry != null)
            {
                if (entry.UserId != TaskComment.UserId)
                {
                    return null;
                }
                #region Check changes
                if (entry.Comment != TaskComment.Comment)
                {
                    entry.Comment = TaskComment.Comment;
                    entry.IsPreviouslyEdited = TaskComment.IsPreviouslyEdited;
                    entry.CommentEditedOn = TaskComment.CommentEditedOn;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }

            return TaskComment;
        }

        #endregion TaskComment





    }
}