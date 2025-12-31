using System;
using System.Data;
using System.Text;
using ARAS.Infrastructure.DBContext;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ARASDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(ARASDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
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
            catch {
                userId = 0;
            }
            return userId;
        }
        public async Task<User> AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            User userFromDB = await _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            return userFromDB;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> GetUserById(long userId)
        {
            User user = await _dbContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            var entry = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (entry != null)
            {
                #region Check changes
                if (entry.EmployeeNumber != user.EmployeeNumber)
                {
                    entry.EmployeeNumber = user.EmployeeNumber;
                }
                if (entry.FirstName != user.FirstName)
                {
                    entry.FirstName = user.FirstName;
                }
                if (entry.MiddleName != user.MiddleName)
                {
                    entry.MiddleName = user.MiddleName;
                }
                if (entry.LastName != user.LastName)
                {
                    entry.LastName = user.LastName;
                }
                if (entry.Metadata != user.Metadata)
                {
                    entry.Metadata = user.Metadata;
                }
                if (entry.PasswordHash != user.PasswordHash)
                {
                    entry.PasswordHash = user.PasswordHash;
                }
                if (entry.PasswordSalt != user.PasswordSalt)
                {
                    entry.PasswordSalt = user.PasswordSalt;
                }
                if (entry.IsEmailConfirmed != user.IsEmailConfirmed)
                {
                    entry.IsEmailConfirmed = user.IsEmailConfirmed;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return entry;
        }



        public async Task<Project> AddProject(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }
        public async Task<Project> UpdateProject(Project project)
        {
            var entry = await _dbContext.Projects
                .FirstOrDefaultAsync(u => u.ProjectId == project.ProjectId);

            if (entry != null)
            {
                #region Check changes
                if (entry.ProjectKey != project.ProjectKey)
                {
                    entry.ProjectKey = project.ProjectKey;
                }
                if (entry.ProjectName != project.ProjectName)
                {
                    entry.ProjectName = project.ProjectName;
                }
                if (entry.Description != project.Description)
                {
                    entry.Description = project.Description;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return project;
        }
        public async Task<List<Project>> GetAllProjects()
        {
            List<Project> projects = await _dbContext.Projects.ToListAsync();
            return projects;
        }
        public async Task<Project> ProjectById(long projectId)
        {
            Project project = await _dbContext.Projects.Where(u => u.ProjectId == projectId).FirstOrDefaultAsync();
            return project;
        }
        public async Task<Project> ProjectByKey(string projectKey)
        {
            Project project = await _dbContext.Projects.Where(u => u.ProjectKey == projectKey).FirstOrDefaultAsync();
            return project;
        }



        public async Task<Role> AddRole(Role role)
        {
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            return role;
        }
        public async Task<Role> UpdateRole(Role role)
        {
            var entry = await _dbContext.Roles
                .FirstOrDefaultAsync(u => u.RoleId == role.RoleId);

            if (entry != null)
            {
                #region Check changes
                if (entry.RoleName != role.RoleName)
                {
                    entry.RoleName = role.RoleName;
                }
                if (entry.Description != role.Description)
                {
                    entry.Description = role.Description;
                }
                if (entry.IsSystemRole != role.IsSystemRole)
                {
                    entry.IsSystemRole = role.IsSystemRole;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return role;
        }
        public async Task<List<Role>> GetAllRoles()
        {
            List<Role> roles = await _dbContext.Roles.ToListAsync();
            return roles;
        }

        public async Task<List<Role>> GetAllRolesLessID(int roleId)
        {
            List<Role> roles = await _dbContext.Roles.Where(u => u.RoleId >= roleId).ToListAsync();
            return roles;
        }
        public async Task<Role> GetRoleById(long roleId)
        {
            Role role = await _dbContext.Roles.Where(u => u.RoleId == roleId).FirstOrDefaultAsync();
            return role;
        }




        public async Task<UserRole> AddUserRole(UserRole userRole)
        {
            await _dbContext.UserRoles.AddAsync(userRole);
            await _dbContext.SaveChangesAsync();

            return userRole;
        }
        public async Task<UserRole> UpdateUserRole(UserRole userRole)
        {
            var entry = await _dbContext.UserRoles
                .FirstOrDefaultAsync(u => u.UserRoleId == userRole.UserRoleId);

            if (entry != null)
            {
                #region Check changes
                //if (entry.UserId != userRole.UserId)
                //{
                //    entry.UserId = userRole.UserId;
                //}
                if (entry.RoleId != userRole.RoleId)
                {
                    entry.RoleId = userRole.RoleId;
                }
                //if (entry.ProjectId != userRole.ProjectId)
                //{
                //    entry.ProjectId = userRole.ProjectId;
                //}
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return userRole;
        }
        public async Task<List<UserRole>> GetAllUserRoles()
        {
            List<UserRole> userRoles = await _dbContext.UserRoles.ToListAsync();
            return userRoles;
        }
        public async Task<UserRole> GetUserRoleById(long userRoleId)
        {
            UserRole userRole = await _dbContext.UserRoles.Where(u => u.UserRoleId == userRoleId).FirstOrDefaultAsync();
            return userRole;
        }
        public async Task<List<UserRole>> GetUserRolesByUserId(long userId)
        {
            List<UserRole> userRoles = await _dbContext.UserRoles.Where(u => u.UserId == userId).ToListAsync();
            return userRoles;
        }

        public async Task<List<UserIdNameModel>> GetUserIdName(bool isAdmin = false)
        {
            List<UserIdNameModel> userIdNameModels;
            if (isAdmin)
            {
                userIdNameModels = await _dbContext.Users.Select(u => new UserIdNameModel
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToListAsync();
                return userIdNameModels;
            }
            userIdNameModels = await _dbContext.Users.Where(
                    u => !_dbContext.UserRoles.Any(r => r.UserId == u.UserId && (r.RoleId == 101 || r.RoleId == 102)))
                    .Select(u => new UserIdNameModel
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }).ToListAsync();
            return userIdNameModels;
        }
        public async Task<UserRole> GetUserRolesByUserAndProjectId(long userId, int projectId)
        {
            UserRole userRole = await _dbContext.UserRoles.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefaultAsync();
            return userRole;
        }
        
        public async Task<bool> DeleteUserRole(long userRoleId)
        {
            await _dbContext.UserRoles.Where(u => u.UserRoleId == userRoleId).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PasswordReset> AddPasswordReset(PasswordReset passwordReset)
        {
            await _dbContext.PasswordResets.AddAsync(passwordReset);
            await _dbContext.SaveChangesAsync();

            return passwordReset;
        }
        public async Task<PasswordReset> GetPasswordReset(string email, int otp)
        {
            PasswordReset passwordReset = await _dbContext.PasswordResets.Where(u => u.Email == email && u.OTP == otp && u.IsVerified == false && u.ValidityDateTime >= DateTimeOffset.UtcNow).FirstOrDefaultAsync();

            return passwordReset;
        }
        public async Task<PasswordReset> UpdatePasswordReset(PasswordReset passwordReset)
        {
            var entry = await _dbContext.PasswordResets
                .FirstOrDefaultAsync(u => u.Id == passwordReset.Id);
            if (entry != null)
            {
                #region Check changes
                if (entry.IsVerified != passwordReset.IsVerified)
                {
                    entry.IsVerified = passwordReset.IsVerified;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }

            return passwordReset;
        }





        public async Task<List<ResourceName>> GetAllResourceNamesByProjectKey(int projectId)
        {
            List<ResourceName> resourceNames = await _dbContext.ResourceNames.Where(u=> u.ProjectId == projectId).ToListAsync();
            return resourceNames;
        }

        public async Task<ResourceName> GetResourceNameByUserId(long userId, int projectId)
        {
            ResourceName resourceName = await _dbContext.ResourceNames.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefaultAsync();
            return resourceName;
        }
        public async Task<ResourceName> AddResourceName(ResourceName resourceName)
        {
            await _dbContext.ResourceNames.AddAsync(resourceName);
            await _dbContext.SaveChangesAsync();
            ResourceName resourceNameDB = await _dbContext.ResourceNames.Where(u => u.GuidId == resourceName.GuidId).FirstOrDefaultAsync();

            return resourceNameDB;
        }
        public async Task<bool> UpdateResourceName(ResourceName resourceName)
        {
            var entry = await _dbContext.ResourceNames
                .FirstOrDefaultAsync(u => u.GuidId == resourceName.GuidId);
            if (entry != null)
            {
                #region Check changes
                if (entry.IsShow != resourceName.IsShow)
                {
                    entry.IsShow = resourceName.IsShow;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }


        public async Task<List<DropDownValueModel>> GetAllDropDownValuesByProjectKey(string projectKey, string type)
        {
            return await _dbContext.DropDownValues.Where(u => u.ProjectKey == projectKey && u.Type == type)
         .Select(u => new DropDownValueModel
         {
             GuidId = u.GuidId,
             Value = u.Value
         })
         .ToListAsync();
        }
        public async Task<DropDownValue> AddDropDownValue(DropDownValue dropDownValue)
        {
            await _dbContext.DropDownValues.AddAsync(dropDownValue);
            await _dbContext.SaveChangesAsync();
            DropDownValue dropDownValueDB = await _dbContext.DropDownValues.Where(u => u.GuidId == dropDownValue.GuidId).FirstOrDefaultAsync();

            return dropDownValueDB;
        }
        public async Task<bool> UpdateDropDownValue(DropDownValue dropDownValue)
        {
            var entry = await _dbContext.DropDownValues
                .FirstOrDefaultAsync(u => u.GuidId == dropDownValue.GuidId);
            if (entry != null)
            {
                #region Check changes
                if (entry.Value != dropDownValue.Value)
                {
                    entry.Value = dropDownValue.Value;
                }
                #endregion Check changes
            }

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteDropDownValue(Guid guidId)
        {
            await _dbContext.DropDownValues.Where(u => u.GuidId == guidId).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            return true;
        }




        

       
        


        




        public async Task<List<DailyTasksLog>> GetTaskLogById(Guid taskId)
        {
            List<DailyTasksLog> dailyTasksLogs = await _dbContext.DailyTasksLogs.Where(u => u.TaskId == taskId).OrderByDescending(u=> u.LogId).ToListAsync();
            return dailyTasksLogs;
        }

        public async Task<List<ProjectAccess>> GetProjectAccess(long userId)
        {
            List<ProjectAccess> projectAccesss = await _dbContext.UserRoles
                .Where(u => u.UserId == userId)
                .Join(_dbContext.Projects,
                      u => u.ProjectId,
                      p => p.ProjectId,
                      (u, p) => new ProjectAccess
                      {
                          ProjectId = p.ProjectId,
                          ProjectKey = p.ProjectKey,
                          ProjectName = p.ProjectName,
                          Description = p.Description
                      })
                .ToListAsync(); 
            return projectAccesss;
        }








        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(FormattableString sql) where T : class
        {
            return await _dbContext.Database.SqlQuery<T>(sql).ToListAsync();
        }

        //public async Task<long> VerifyProjectAccessAndGetRoleId(string userId, string projectKey)
        //{
        //    using var command = _dbContext.Database.GetDbConnection().CreateCommand();
        //    command.CommandText = "VerifyProjectAccessAndGetRoleId";
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add(new SqlParameter("@UserId", int.Parse(userId)));
        //    command.Parameters.Add(new SqlParameter("@ProjectKey", projectKey));

        //    await _dbContext.Database.OpenConnectionAsync();

        //    long result = (long)(await command.ExecuteScalarAsync() ?? 0);

        //    return result;
        //}
    }
}

