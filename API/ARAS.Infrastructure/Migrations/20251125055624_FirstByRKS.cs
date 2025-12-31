using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ARAS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstByRKS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTasks",
                columns: table => new
                {
                    TaskId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskSeq = table.Column<int>(type: "int", nullable: false),
                    Catagory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubProject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TodayDayWork = table.Column<bool>(type: "bit", nullable: false),
                    ItemToDiscuss = table.Column<bool>(type: "bit", nullable: false),
                    EHrsToday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MyComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jira = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "DailyTasksLogs",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTasksLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "DropDownValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    OTP = table.Column<int>(type: "int", nullable: false),
                    ValidityDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    FailedAttempts = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "5001, 1"),
                    ProjectKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ResourceNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false),
                    AddedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    SubTaskId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubTaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTaskSeq = table.Column<int>(type: "int", nullable: false),
                    SubTaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTaskETA = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsColour = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    IsSelf = table.Column<bool>(type: "bit", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.SubTaskId);
                });

            migrationBuilder.CreateTable(
                name: "SubTaskWorkEntrys",
                columns: table => new
                {
                    WorkEntryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkEntryUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EntryHrs = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTaskWorkEntrys", x => x.WorkEntryId);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskUniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsPreviouslyEdited = table.Column<bool>(type: "bit", nullable: false),
                    CommentEditedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProjectKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.GuidId);
                });

            migrationBuilder.CreateTable(
                name: "UseFullLinks",
                columns: table => new
                {
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseFullLinks", x => x.GuidId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "801, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRoles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DropDownValues",
                columns: new[] { "Id", "GuidId", "ProjectKey", "Type", "Value" },
                values: new object[,]
                {
                    { 1, new Guid("11111111-1111-1111-1111-111111111111"), "cpp", "project", "CPP" },
                    { 2, new Guid("22222222-2222-2222-2222-222222222222"), "cpp", "project", "PPG" },
                    { 3, new Guid("33333333-3333-3333-3333-333333333333"), "cpp", "project", "TC" },
                    { 4, new Guid("44444444-4444-4444-4444-444444444444"), "ccp", "project", "CCP" },
                    { 5, new Guid("55555555-5555-5555-5555-555555555555"), "plat", "project", "PLAT" },
                    { 6, new Guid("66666666-6666-6666-6666-666666666666"), "cpp", "network", "MDS" },
                    { 7, new Guid("77777777-7777-7777-7777-777777777777"), "cpp", "network", "Visa" },
                    { 8, new Guid("88888888-8888-8888-8888-888888888888"), "cpp", "network", "Pulse_ED" },
                    { 9, new Guid("99999999-9999-9999-9999-999999999999"), "cpp", "network", "Star" },
                    { 10, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "cpp", "network", "Discover" },
                    { 11, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "cpp", "network", "IPM" },
                    { 12, new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "cpp", "network", "RupaySms" },
                    { 13, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "cpp", "network", "RupayDms" },
                    { 14, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "cpp", "network", "Jaywan" },
                    { 15, new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "ccp", "network", "VisaSms" },
                    { 16, new Guid("12121212-1212-1212-1212-121212121212"), "ccp", "network", "VisaDms" },
                    { 17, new Guid("13131313-1313-1313-1313-131313131313"), "ccp", "network", "IPM" },
                    { 18, new Guid("14141414-1414-1414-1414-141414141414"), "ccp", "network", "AmexDef" },
                    { 19, new Guid("15151515-1515-1515-1515-151515151515"), "ccp", "network", "AmexAch" },
                    { 20, new Guid("16161616-1616-1616-1616-161616161616"), "ccp", "network", "Discover" },
                    { 21, new Guid("17171717-1717-1717-1717-171717171717"), "ccp", "network", "RupaySms" },
                    { 22, new Guid("18181818-1818-1818-1818-181818181818"), "ccp", "network", "RupayDms" },
                    { 23, new Guid("19191919-1919-1919-1919-191919191919"), "plat", "network", "IPM" },
                    { 24, new Guid("20202020-2020-2020-2020-202020202020"), "plat", "category", "Internal" },
                    { 25, new Guid("21212121-2121-2121-2121-212121212121"), "cpp", "category", "Internal" },
                    { 26, new Guid("22222222-2222-2222-2222-212121212121"), "ccp", "category", "Internal" },
                    { 27, new Guid("23232323-2323-2323-2323-232323232323"), "ccp", "category", "Prod" },
                    { 28, new Guid("24242424-2424-2424-2424-242424242424"), "cpp", "category", "Prod" },
                    { 29, new Guid("25252525-2525-2525-2525-252525252525"), "plat", "category", "Prod" },
                    { 30, new Guid("26262626-2626-2626-2626-262626262626"), "plat", "category", "Regular" },
                    { 31, new Guid("27272727-2727-2727-2727-272727272727"), "cpp", "category", "Regular" },
                    { 32, new Guid("28282828-2828-2828-2828-282828282828"), "ccp", "category", "Regular" },
                    { 33, new Guid("29292929-2929-2929-2929-292929292929"), "cpp", "status", "Pending" },
                    { 34, new Guid("30303030-3030-3030-3030-303030303030"), "cpp", "status", "In-Progress" },
                    { 35, new Guid("31313131-3131-3131-3131-313131313131"), "cpp", "status", "Done" },
                    { 36, new Guid("32323232-3232-3232-3232-323232323232"), "cpp", "status", "On-Hold" },
                    { 37, new Guid("33333333-3333-3333-3333-333333333333"), "cpp", "status", "Follow-Up" },
                    { 38, new Guid("34343434-3434-3434-3434-343434343434"), "ccp", "status", "Pending" },
                    { 39, new Guid("35353535-3535-3535-3535-353535353535"), "ccp", "status", "In-Progress" },
                    { 40, new Guid("36363636-3636-3636-3636-363636363636"), "ccp", "status", "Done" },
                    { 41, new Guid("37373737-3737-3737-3737-373737373737"), "ccp", "status", "On-Hold" },
                    { 42, new Guid("38383838-3838-3838-3838-383838383838"), "ccp", "status", "Follow-Up" },
                    { 43, new Guid("39393939-3939-3939-3939-393939393939"), "plat", "status", "Pending" },
                    { 44, new Guid("40404040-4040-4040-4040-404040404040"), "plat", "status", "In-Progress" },
                    { 45, new Guid("41414141-4141-4141-4141-414141414141"), "plat", "status", "Done" },
                    { 46, new Guid("42424242-4242-4242-4242-424242424242"), "plat", "status", "On-Hold" },
                    { 47, new Guid("43434343-4343-4343-4343-434343434343"), "plat", "status", "Follow-Up" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CreatedAt", "Description", "ProjectKey", "ProjectName" },
                values: new object[,]
                {
                    { 5001, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Have networks - MDS, Visa Sms, Pulse EDF, Star, IPM, Discover, Rupay Sms & Dms, Jaywan", "cpp", "Core Prepaid Processing" },
                    { 5002, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Have networks - Visa Sms, Amex, IPM, Discover, Rupay Sms & Dms, Jaywan", "ccp", "Core Credit Processing" },
                    { 5003, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Have network - Only Mastercard IPM", "plat", "Plat Processing" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "IsSystemRole", "RoleName" },
                values: new object[,]
                {
                    { 101, "", false, "admin" },
                    { 102, "", false, "manager" },
                    { 103, "", false, "lead" },
                    { 104, "", false, "resource" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "EmployeeNumber", "FailedLoginCount", "FirstName", "IsActive", "IsEmailConfirmed", "LastLoginAt", "LastName", "LockedUntil", "Metadata", "MiddleName", "PasswordHash", "PasswordSalt", "UpdatedAt", "UserGuid", "Username" },
                values: new object[] { 1001L, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin", 0L, 0, "Admin", true, true, null, "Admin", null, "", "", "jjVzvvxb4B1w2/P8vByljzlH3CsIgBYLGEbfP7MWBdw=", "RW6uOLKZH7GrVfzOeNtDjA==", null, new Guid("683869d4-10af-4a94-a520-0bf2470d28a6"), "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "AssignedAt", "ProjectId", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 801L, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5001, 101, 1001L },
                    { 802L, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5002, 101, 1001L },
                    { 803L, new DateTimeOffset(new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5003, 101, 1001L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ProjectId",
                table: "UserRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            var path = Path.Combine("..", "ARAS.Infrastructure", "SQL_Scripts", "Tables.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
            path = Path.Combine("..", "ARAS.Infrastructure", "SQL_Scripts", "SPs.sql");
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTasks");

            migrationBuilder.DropTable(
                name: "DailyTasksLogs");

            migrationBuilder.DropTable(
                name: "DropDownValues");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropTable(
                name: "ResourceNames");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "SubTaskWorkEntrys");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "UseFullLinks");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
