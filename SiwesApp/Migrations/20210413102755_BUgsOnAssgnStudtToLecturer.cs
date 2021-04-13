using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class BUgsOnAssgnStudtToLecturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignStudentToLecturer_IndustrialSupervisors_IndustrialSupervisorId",
                table: "AssignStudentToLecturer");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignStudentToLecturer_Lecturers_LecturerId",
                table: "AssignStudentToLecturer");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AssignStudentToLecturer_AssignStudentToLecturerId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignStudentToLecturer",
                table: "AssignStudentToLecturer");

            migrationBuilder.RenameTable(
                name: "AssignStudentToLecturer",
                newName: "AssignStudentToLecturers");

            migrationBuilder.RenameIndex(
                name: "IX_AssignStudentToLecturer_LecturerId",
                table: "AssignStudentToLecturers",
                newName: "IX_AssignStudentToLecturers_LecturerId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignStudentToLecturer_IndustrialSupervisorId",
                table: "AssignStudentToLecturers",
                newName: "IX_AssignStudentToLecturers_IndustrialSupervisorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignStudentToLecturers",
                table: "AssignStudentToLecturers",
                column: "AssignStudentToLecturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignStudentToLecturers_IndustrialSupervisors_IndustrialSupervisorId",
                table: "AssignStudentToLecturers",
                column: "IndustrialSupervisorId",
                principalTable: "IndustrialSupervisors",
                principalColumn: "IndustrialSupervisorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignStudentToLecturers_Lecturers_LecturerId",
                table: "AssignStudentToLecturers",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AssignStudentToLecturers_AssignStudentToLecturerId",
                table: "Students",
                column: "AssignStudentToLecturerId",
                principalTable: "AssignStudentToLecturers",
                principalColumn: "AssignStudentToLecturerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignStudentToLecturers_IndustrialSupervisors_IndustrialSupervisorId",
                table: "AssignStudentToLecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignStudentToLecturers_Lecturers_LecturerId",
                table: "AssignStudentToLecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AssignStudentToLecturers_AssignStudentToLecturerId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignStudentToLecturers",
                table: "AssignStudentToLecturers");

            migrationBuilder.RenameTable(
                name: "AssignStudentToLecturers",
                newName: "AssignStudentToLecturer");

            migrationBuilder.RenameIndex(
                name: "IX_AssignStudentToLecturers_LecturerId",
                table: "AssignStudentToLecturer",
                newName: "IX_AssignStudentToLecturer_LecturerId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignStudentToLecturers_IndustrialSupervisorId",
                table: "AssignStudentToLecturer",
                newName: "IX_AssignStudentToLecturer_IndustrialSupervisorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignStudentToLecturer",
                table: "AssignStudentToLecturer",
                column: "AssignStudentToLecturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignStudentToLecturer_IndustrialSupervisors_IndustrialSupervisorId",
                table: "AssignStudentToLecturer",
                column: "IndustrialSupervisorId",
                principalTable: "IndustrialSupervisors",
                principalColumn: "IndustrialSupervisorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignStudentToLecturer_Lecturers_LecturerId",
                table: "AssignStudentToLecturer",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AssignStudentToLecturer_AssignStudentToLecturerId",
                table: "Students",
                column: "AssignStudentToLecturerId",
                principalTable: "AssignStudentToLecturer",
                principalColumn: "AssignStudentToLecturerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
