using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class AddingGradeAndCommentToLogBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IndustrialSupervisorComment",
                table: "LogBooks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustrialSupervisorGrade",
                table: "LogBooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LecturerComment",
                table: "LogBooks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LecturerGrade",
                table: "LogBooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndustrialSupervisorComment",
                table: "LogBooks");

            migrationBuilder.DropColumn(
                name: "IndustrialSupervisorGrade",
                table: "LogBooks");

            migrationBuilder.DropColumn(
                name: "LecturerComment",
                table: "LogBooks");

            migrationBuilder.DropColumn(
                name: "LecturerGrade",
                table: "LogBooks");
        }
    }
}
