using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Infrastructure.DBModels
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    namespace YourApp.DomainModels
    {
        [Table("Projects")]
        public class Project
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProjectId { get; set; }
            [StringLength(50)]
            public string ProjectKey { get; set; } = string.Empty;
            [StringLength(50)]
            public string ProjectName { get; set; } = string.Empty;

            [StringLength(500)]
            public string Description { get; set; } = string.Empty;
            public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        }

        [Table("Users")]
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long UserId { get; set; }
            public Guid UserGuid { get; set; } = Guid.NewGuid();
            [StringLength(100)]
            public string Username { get; set; } = string.Empty;
            [StringLength(100)]
            public string FirstName { get; set; } = string.Empty;
            [StringLength(100)]
            public string MiddleName { get; set; } = string.Empty;
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;
            public long EmployeeNumber { get; set; }
            [StringLength(225)]
            public string Email { get; set; } = string.Empty;
            [StringLength(512)]
            public string PasswordHash { get; set; } = string.Empty;

            [StringLength(512)]
            public string PasswordSalt { get; set; } = string.Empty;
            public bool IsActive { get; set; } = true;
            public bool IsEmailConfirmed { get; set; } = false;
            public int FailedLoginCount { get; set; } = 0;
            public DateTimeOffset? LockedUntil { get; set; }
            public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
            public DateTimeOffset? UpdatedAt { get; set; }
            public DateTimeOffset? LastLoginAt { get; set; }
            public string Metadata { get; set; } = string.Empty;
        }

        [Table("Roles")]
        public class Role
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int RoleId { get; set; }
            [StringLength(100)]
            public string RoleName { get; set; } = string.Empty;
            [StringLength(500)]
            public string Description { get; set; } = string.Empty;
            public bool IsSystemRole { get; set; } = false;
        }

        [Table("UserRoles")]
        public class UserRole
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long UserRoleId { get; set; }
            public long UserId { get; set; }
            public int RoleId { get; set; }
            public int ProjectId { get; set; }
            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;

            // Navigation properties
            public User User { get; set; } = null!;
            public Role Role { get; set; } = null!;
            public Project Project { get; set; } = null!;
        }

        [Table("PasswordResets")]
        public class PasswordReset
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }
            public long UserId { get; set; }
            [StringLength(225)]
            public string Email { get; set; } = string.Empty;
            public int OTP { get; set; }
            public DateTimeOffset ValidityDateTime { get; set; }
            public bool IsVerified { get; set; } = false;
            public int FailedAttempts { get; set; } = 0;
            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;
        }

        [Table("DailyTasks")]
        public class DailyTask
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long TaskId { get; set; }
            public long UserId { get; set; }
            [StringLength(100)]
            public string UserName { get; set; } = string.Empty;
            public Guid TaskUniqueId { get; set; } = Guid.NewGuid();
            public int TaskSeq { get; set; }
            [StringLength(100)]
            public string Catagory { get; set; } = string.Empty;
            public string TaskName { get; set; } = string.Empty;
            [StringLength(50)]
            public string SubProject { get; set; } = string.Empty;
            [StringLength(50)]
            public string ProjectKey { get; set; } = string.Empty;
            public int ProjectId { get; set; }
            [StringLength(100)]
            public string Network { get; set; } = string.Empty;
            [StringLength(100)]
            public string Status { get; set; } = string.Empty;
            public bool TodayDayWork { get; set; } = false;
            public bool ItemToDiscuss { get; set; } = false;
            public string EHrsToday { get; set; } = string.Empty;
            public string MyComments { get; set; } = string.Empty;
            public string ManagerComments { get; set; } = string.Empty;
            [StringLength(100)]
            public string Jira { get; set; } = "N.A.";
            public bool IsApprove { get; set; } = false;
            public bool IsAlive { get; set; } = true;
            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;
            public Guid RNGuidId { get; set; } 
            public string FeatureName { get; set; } = string.Empty;
            [StringLength(100)]
            public string FixVersion { get; set; } = string.Empty;
            public string RNComments { get; set; } = string.Empty;
            public string MailTitled { get; set; } = string.Empty;
        }

        [Table("DailyTasksLogs")]
        public class DailyTasksLog
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long LogId { get; set; }
            public Guid TaskId { get; set; }
            public string Log { get; set; } = string.Empty;
            public long UserId { get; set; }
            public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        }
        [Table("SubTasks")]
        public class SubTask
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long SubTaskId { get; set; }
            public Guid SubTaskUniqueId { get; set; } = Guid.NewGuid();
            public Guid TaskUniqueId { get; set; }
            public int SubTaskSeq { get; set; }
            public string SubTaskName { get; set; } = string.Empty;
            public string Remark { get; set; } = string.Empty; 
            [Precision(5, 1)]
            public decimal SubTaskETA { get; set; } = 0;
            [StringLength(100)]
            public string Status { get; set; } = string.Empty;
            public bool IsColour { get; set; } = false;
            public long UserId { get; set; }
            public bool IsApprove { get; set; } = false;
            public bool IsSelf { get; set; } = true;

            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;
        }
        [Table("SubTaskWorkEntrys")]
        public class SubTaskWorkEntry
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long WorkEntryId { get; set; }
            public Guid WorkEntryUniqueId { get; set; } = Guid.NewGuid();
            public Guid SubTaskUniqueId { get; set; }
            public Guid TaskUniqueId { get; set; }

            public DateOnly EntryDate { get; set; }
            [Precision(5, 1)]
            public decimal EntryHrs { get; set; }
            public long UserId { get; set; }
            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;

        }
        [Table("ResourceNames")]
        public class ResourceName
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public Guid GuidId { get; set; } = Guid.NewGuid();
            public long UserId { get; set; }
            [StringLength(100)]
            public string UserName { get; set; } = string.Empty;
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;
            public int ProjectId { get; set; }
            public bool IsShow {  get; set; } = true;
            public long AddedByUserId { get; set; }
            public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        }
        [Table("DropDownValues")]
        public class DropDownValue
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public Guid GuidId { get; set; } = Guid.NewGuid();
            [StringLength(50)]
            public string ProjectKey { get; set; } = string.Empty;
            [StringLength(200)]
            public string Type { get; set; }

            [StringLength(200)]
            public string Value { get; set; }
        }

        [Table("UseFullLinks")]
        public class UseFullLink
        {
            [Key]
            public Guid GuidId { get; set; } = Guid.NewGuid();
            public string LinkName { get; set; } = string.Empty;
            public string LinkURL { get; set; } = string.Empty;
            public string ProjectKey { get; set; } = string.Empty;
            public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.Now;
        }



        [Table("TaskComments")]
        public class TaskComment
        {
            [Key]
            public Guid GuidId { get; set; } = Guid.NewGuid();

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int CommentId { get; set; }
            public Guid TaskUniqueId { get; set; }
            public string FirstName { get; set; } = string.Empty;

            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;
            public long UserId { get; set; } = 0;
            public string Comment { get; set; } = string.Empty;
            public DateTimeOffset CommentedOn { get; set; } = DateTimeOffset.Now;
            public bool IsPreviouslyEdited { get; set; } = false;
            public DateTimeOffset? CommentEditedOn { get; set; } = null;
            public string ProjectKey { get; set; } = string.Empty;
        }

        [Table("RNAndFeatureList")]
        public class RNAndFeature
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public Guid GuidId { get; set; } = Guid.NewGuid();
            [StringLength(100)]
            public string RN { get; set; } = string.Empty;
            [StringLength(50)]
            public string ProjectKey { get; set; } = string.Empty;
            public bool IsShow { get; set; } = true;
            public long AddedByUserId { get; set; }
            public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        }

        public class UseFullLinkModel
        {
            public Guid GuidId { get; set; }
            public string LinkName { get; set; }
            public string LinkURL { get; set; }
        }


        public class DropDownValueModel
        {
            public Guid GuidId { get; set; }
            public string Value { get; set; }
        }
        public class UserIdNameModel
        {
            public long UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public class ProjectAccess
        {
            public int ProjectId { get; set; }
            public string ProjectKey { get; set; }
            public string ProjectName { get; set; }
            public string Description { get; set; }
        }

    }

}
