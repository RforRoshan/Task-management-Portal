using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARAS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class third1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "RNAndFeatureList");

            migrationBuilder.AddColumn<string>(
                name: "ProjectKey",
                table: "RNAndFeatureList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectKey",
                table: "RNAndFeatureList");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "RNAndFeatureList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
