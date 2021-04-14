using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class ImplimentingLogBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogBooks",
                columns: table => new
                {
                    LogBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    Day = table.Column<DateTimeOffset>(nullable: false),
                    Time = table.Column<DateTimeOffset>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogBooks", x => x.LogBookId);
                    table.ForeignKey(
                        name: "FK_LogBooks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogBooks_StudentId",
                table: "LogBooks",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogBooks");
        }
    }
}
