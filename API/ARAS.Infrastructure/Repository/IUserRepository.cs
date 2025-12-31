using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Infrastructure.Repository
{
    public interface IUserRepository
    {
        public  Task<User> AddUser(User user);
        public  Task<User> GetUserByEmail(string email);
        public Task<User> GetUserById(long userId);
        public  Task<User> UpdateUser(User user);
        public  Task<Project> AddProject(Project project);
        public  Task<Project> UpdateProject(Project project);
        public  Task<List<Project>> GetAllProjects();
        public  Task<Project> ProjectById(long projectId);
        public  Task<Project> ProjectByKey(string projectKey);

        public  Task<Role> AddRole(Role role);
        public  Task<Role> UpdateRole(Role role);
        public  Task<List<Role>> GetAllRoles();
        public Task<List<Role>> GetAllRolesLessID(int roleId);
        public  Task<Role> GetRoleById(long roleId);

        public  Task<UserRole> AddUserRole(UserRole userRole);
        public  Task<UserRole> UpdateUserRole(UserRole userRole);
        public  Task<List<UserRole>> GetAllUserRoles();
        public  Task<UserRole> GetUserRoleById(long userRoleId);
        public Task<List<UserIdNameModel>> GetUserIdName(bool isAdmin = false);
        public  Task<List<UserRole>> GetUserRolesByUserId(long userId);
        public Task<UserRole> GetUserRolesByUserAndProjectId(long userId, int projectId);
        public Task<bool> DeleteUserRole(long userRoleId);

        public Task<PasswordReset> AddPasswordReset(PasswordReset passwordReset);
        public Task<PasswordReset> GetPasswordReset(string email, int otp);
        public Task<PasswordReset> UpdatePasswordReset(PasswordReset passwordReset);


        public Task<List<ResourceName>> GetAllResourceNamesByProjectKey(int projectId);
        public Task<ResourceName> GetResourceNameByUserId(long userId, int projectId);
        public Task<ResourceName> AddResourceName(ResourceName resourceName);
        public Task<bool> UpdateResourceName(ResourceName resourceName);


        public Task<List<DropDownValueModel>> GetAllDropDownValuesByProjectKey(string projectKey, string type);
        public Task<DropDownValue> AddDropDownValue(DropDownValue dropDownValue);
        public Task<bool> UpdateDropDownValue(DropDownValue dropDownValue);
        public Task<bool> DeleteDropDownValue(Guid guidId);



        public Task<List<ProjectAccess>> GetProjectAccess(long userId);
        public Task<List<DailyTasksLog>> GetTaskLogById(Guid taskId);



        

        public Task<List<T>> ExecuteStoredProcedureAsync<T>(FormattableString sql) where T : class;

        //public Task<long> VerifyProjectAccessAndGetRoleId(string userId, string projectKey);

    }
}
