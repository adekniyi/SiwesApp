using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class AssignStudentToLecturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignStudentToLecturerId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssignStudentToLecturer",
                columns: table => new
                {
                    AssignStudentToLecturerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    LecturerId = table.Column<int>(nullable: false),
                    IndustrialSupervisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignStudentToLecturer", x => x.AssignStudentToLecturerId);
                    table.ForeignKey(
                        name: "FK_AssignStudentToLecturer_IndustrialSupervisors_IndustrialSupervisorId",
                        column: x => x.IndustrialSupervisorId,
                        principalTable: "IndustrialSupervisors",
                        principalColumn: "IndustrialSupervisorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignStudentToLecturer_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AssignStudentToLecturerId",
                table: "Students",
                column: "AssignStudentToLecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignStudentToLecturer_IndustrialSupervisorId",
                table: "AssignStudentToLecturer",
                column: "IndustrialSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignStudentToLecturer_LecturerId",
                table: "AssignStudentToLecturer",
                column: "LecturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AssignStudentToLecturer_AssignStudentToLecturerId",
                table: "Students",
                column: "AssignStudentToLecturerId",
                principalTable: "AssignStudentToLecturer",
                principalColumn: "AssignStudentToLecturerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AssignStudentToLecturer_AssignStudentToLecturerId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "AssignStudentToLecturer");

            migrationBuilder.DropIndex(
                name: "IX_Students_AssignStudentToLecturerId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AssignStudentToLecturerId",
                table: "Students");
        }
    }
}
