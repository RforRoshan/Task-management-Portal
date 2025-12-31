using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Infrastructure.DBContext
{
    public class ARASDbContext : DbContext
    {
        public ARASDbContext(DbContextOptions<ARASDbContext> options):base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<DailyTasksLog> DailyTasksLogs { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<SubTaskWorkEntry> SubTaskWorkEntrys { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<ResourceName> ResourceNames { get; set; }
        public DbSet<DropDownValue> DropDownValues { get; set; }
        public DbSet<UseFullLink> UseFullLinks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<RNAndFeature> RNAndFeatureList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectId)
                .UseIdentityColumn(seed: 5001, increment: 1);


            modelBuilder.Entity<User>()
               .Property(p => p.UserId)
               .UseIdentityColumn(seed: 1001, increment: 1);

            modelBuilder.Entity<Role>()
               .Property(p => p.RoleId)
               .UseIdentityColumn(seed: 101, increment: 1);

            modelBuilder.Entity<UserRole>()
               .Property(p => p.UserRoleId)
               .UseIdentityColumn(seed: 801, increment: 1);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().HasData(
            new Project
                {
                    ProjectId = 5001,
                    ProjectKey = "cpp",
                    ProjectName = "Core Prepaid Processing",
                    Description = "Have networks - MDS, Visa Sms, Pulse EDF, Star, IPM, Discover, Rupay Sms & Dms, Jaywan",
                    CreatedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
            },
                new Project
                {
                    ProjectId = 5002,
                    ProjectKey = "ccp",
                    ProjectName = "Core Credit Processing",
                    Description = "Have networks - Visa Sms, Amex, IPM, Discover, Rupay Sms & Dms, Jaywan",
                    CreatedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
                },
                new Project
                {
                    ProjectId = 5003,
                    ProjectKey = "plat",
                    ProjectName = "Plat Processing",
                    Description = "Have network - Only Mastercard IPM",
                    CreatedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
                }
            );

            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1001,
                UserGuid = new Guid("683869d4-10af-4a94-a520-0bf2470d28a6"),
                Username = "admin",
                FirstName = "Admin",
                MiddleName = "",
                LastName = "Admin",
                EmployeeNumber = 0,
                Email = "admin",
                PasswordHash = "jjVzvvxb4B1w2/P8vByljzlH3CsIgBYLGEbfP7MWBdw=",
                PasswordSalt = "RW6uOLKZH7GrVfzOeNtDjA==",
                IsActive = true,
                IsEmailConfirmed = true,
                FailedLoginCount = 0,
                LockedUntil = null,
                CreatedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero),
                UpdatedAt = null,
                LastLoginAt = null,
                Metadata = ""
            }
            );

            modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 101,
                RoleName = "admin",
                Description = "",
                IsSystemRole = false
            },
            new Role
            {
                RoleId = 102,
                RoleName = "manager",
                Description = "",
                IsSystemRole = false
            },
            new Role
            {
                RoleId = 103,
                RoleName = "lead",
                Description = "",
                IsSystemRole = false
            },
            new Role
            {
                RoleId = 104,
                RoleName = "resource",
                Description = "",
                IsSystemRole = false
            }
            );

            modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserRoleId = 801,
                UserId = 1001,
                RoleId = 101,
                ProjectId = 5001,
                AssignedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
            },
            new UserRole
            {
                UserRoleId = 802,
                UserId = 1001,
                RoleId = 101,
                ProjectId = 5002,
                AssignedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
            },
            new UserRole
            {
                UserRoleId = 803,
                UserId = 1001,
                RoleId = 101,
                ProjectId = 5003,
                AssignedAt = new DateTimeOffset(new DateTime(2025, 10, 30), TimeSpan.Zero)
            }
            );

            modelBuilder.Entity<DropDownValue>().HasData(
                new DropDownValue { Id = 1, ProjectKey = "cpp", Type = "project", Value = "CPP", GuidId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new DropDownValue { Id = 2, ProjectKey = "cpp", Type = "project", Value = "PPG", GuidId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                new DropDownValue { Id = 3, ProjectKey = "cpp", Type = "project", Value = "TC", GuidId = Guid.Parse("33333333-3333-3333-3333-333333333333") },
                new DropDownValue { Id = 4, ProjectKey = "ccp", Type = "project", Value = "CCP", GuidId = Guid.Parse("44444444-4444-4444-4444-444444444444") },
                new DropDownValue { Id = 5, ProjectKey = "plat", Type = "project", Value = "PLAT", GuidId = Guid.Parse("55555555-5555-5555-5555-555555555555") },

                new DropDownValue { Id = 6, ProjectKey = "cpp", Type = "network", Value = "MDS", GuidId = Guid.Parse("66666666-6666-6666-6666-666666666666") },
                new DropDownValue { Id = 7, ProjectKey = "cpp", Type = "network", Value = "Visa", GuidId = Guid.Parse("77777777-7777-7777-7777-777777777777") },
                new DropDownValue { Id = 8, ProjectKey = "cpp", Type = "network", Value = "Pulse_ED", GuidId = Guid.Parse("88888888-8888-8888-8888-888888888888") },
                new DropDownValue { Id = 9, ProjectKey = "cpp", Type = "network", Value = "Star", GuidId = Guid.Parse("99999999-9999-9999-9999-999999999999") },
                new DropDownValue { Id = 10, ProjectKey = "cpp", Type = "network", Value = "Discover", GuidId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                new DropDownValue { Id = 11, ProjectKey = "cpp", Type = "network", Value = "IPM", GuidId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") },
                new DropDownValue { Id = 12, ProjectKey = "cpp", Type = "network", Value = "RupaySms", GuidId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                new DropDownValue { Id = 13, ProjectKey = "cpp", Type = "network", Value = "RupayDms", GuidId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                new DropDownValue { Id = 14, ProjectKey = "cpp", Type = "network", Value = "Jaywan", GuidId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                new DropDownValue { Id = 15, ProjectKey = "ccp", Type = "network", Value = "VisaSms", GuidId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
                new DropDownValue { Id = 16, ProjectKey = "ccp", Type = "network", Value = "VisaDms", GuidId = Guid.Parse("12121212-1212-1212-1212-121212121212") },
                new DropDownValue { Id = 17, ProjectKey = "ccp", Type = "network", Value = "IPM", GuidId = Guid.Parse("13131313-1313-1313-1313-131313131313") },
                new DropDownValue { Id = 18, ProjectKey = "ccp", Type = "network", Value = "AmexDef", GuidId = Guid.Parse("14141414-1414-1414-1414-141414141414") },
                new DropDownValue { Id = 19, ProjectKey = "ccp", Type = "network", Value = "AmexAch", GuidId = Guid.Parse("15151515-1515-1515-1515-151515151515") },
                new DropDownValue { Id = 20, ProjectKey = "ccp", Type = "network", Value = "Discover", GuidId = Guid.Parse("16161616-1616-1616-1616-161616161616") },
                new DropDownValue { Id = 21, ProjectKey = "ccp", Type = "network", Value = "RupaySms", GuidId = Guid.Parse("17171717-1717-1717-1717-171717171717") },
                new DropDownValue { Id = 22, ProjectKey = "ccp", Type = "network", Value = "RupayDms", GuidId = Guid.Parse("18181818-1818-1818-1818-181818181818") },
                new DropDownValue { Id = 23, ProjectKey = "plat", Type = "network", Value = "IPM", GuidId = Guid.Parse("19191919-1919-1919-1919-191919191919") },

                new DropDownValue { Id = 24, ProjectKey = "plat", Type = "category", Value = "Internal", GuidId = Guid.Parse("20202020-2020-2020-2020-202020202020") },
                new DropDownValue { Id = 25, ProjectKey = "cpp", Type = "category", Value = "Internal", GuidId = Guid.Parse("21212121-2121-2121-2121-212121212121") },
                new DropDownValue { Id = 26, ProjectKey = "ccp", Type = "category", Value = "Internal", GuidId = Guid.Parse("22222222-2222-2222-2222-212121212121") },
                new DropDownValue { Id = 27, ProjectKey = "ccp", Type = "category", Value = "Prod", GuidId = Guid.Parse("23232323-2323-2323-2323-232323232323") },
                new DropDownValue { Id = 28, ProjectKey = "cpp", Type = "category", Value = "Prod", GuidId = Guid.Parse("24242424-2424-2424-2424-242424242424") },
                new DropDownValue { Id = 29, ProjectKey = "plat", Type = "category", Value = "Prod", GuidId = Guid.Parse("25252525-2525-2525-2525-252525252525") },
                new DropDownValue { Id = 30, ProjectKey = "plat", Type = "category", Value = "Regular", GuidId = Guid.Parse("26262626-2626-2626-2626-262626262626") },
                new DropDownValue { Id = 31, ProjectKey = "cpp", Type = "category", Value = "Regular", GuidId = Guid.Parse("27272727-2727-2727-2727-272727272727") },
                new DropDownValue { Id = 32, ProjectKey = "ccp", Type = "category", Value = "Regular", GuidId = Guid.Parse("28282828-2828-2828-2828-282828282828") },

                new DropDownValue { Id = 33, ProjectKey = "cpp", Type = "status", Value = "Pending", GuidId = Guid.Parse("29292929-2929-2929-2929-292929292929") },
                new DropDownValue { Id = 34, ProjectKey = "cpp", Type = "status", Value = "In-Progress", GuidId = Guid.Parse("30303030-3030-3030-3030-303030303030") },
                new DropDownValue { Id = 35, ProjectKey = "cpp", Type = "status", Value = "Done", GuidId = Guid.Parse("31313131-3131-3131-3131-313131313131") },
                new DropDownValue { Id = 36, ProjectKey = "cpp", Type = "status", Value = "On-Hold", GuidId = Guid.Parse("32323232-3232-3232-3232-323232323232") },
                new DropDownValue { Id = 37, ProjectKey = "cpp", Type = "status", Value = "Follow-Up", GuidId = Guid.Parse("33333333-3333-3333-3333-333333333333") },

                new DropDownValue { Id = 38, ProjectKey = "ccp", Type = "status", Value = "Pending", GuidId = Guid.Parse("34343434-3434-3434-3434-343434343434") },
                new DropDownValue { Id = 39, ProjectKey = "ccp", Type = "status", Value = "In-Progress", GuidId = Guid.Parse("35353535-3535-3535-3535-353535353535") },
                new DropDownValue { Id = 40, ProjectKey = "ccp", Type = "status", Value = "Done", GuidId = Guid.Parse("36363636-3636-3636-3636-363636363636") },
                new DropDownValue { Id = 41, ProjectKey = "ccp", Type = "status", Value = "On-Hold", GuidId = Guid.Parse("37373737-3737-3737-3737-373737373737") },
                new DropDownValue { Id = 42, ProjectKey = "ccp", Type = "status", Value = "Follow-Up", GuidId = Guid.Parse("38383838-3838-3838-3838-383838383838") },

                new DropDownValue { Id = 43, ProjectKey = "plat", Type = "status", Value = "Pending", GuidId = Guid.Parse("39393939-3939-3939-3939-393939393939") },
                new DropDownValue { Id = 44, ProjectKey = "plat", Type = "status", Value = "In-Progress", GuidId = Guid.Parse("40404040-4040-4040-4040-404040404040") },
                new DropDownValue { Id = 45, ProjectKey = "plat", Type = "status", Value = "Done", GuidId = Guid.Parse("41414141-4141-4141-4141-414141414141") },
                new DropDownValue { Id = 46, ProjectKey = "plat", Type = "status", Value = "On-Hold", GuidId = Guid.Parse("42424242-4242-4242-4242-424242424242") },
                new DropDownValue { Id = 47, ProjectKey = "plat", Type = "status", Value = "Follow-Up", GuidId = Guid.Parse("43434343-4343-4343-4343-434343434343") }
            );
        }
    }
}
