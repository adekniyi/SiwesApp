using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class Placemnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EligiblityStatus",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Placements",
                columns: table => new
                {
                    PlacementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    MatricNumber = table.Column<int>(nullable: false),
                    RegistrationNumber = table.Column<int>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    Programm = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    OfferLetter = table.Column<string>(nullable: true),
                    StudentPicture = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    SectionOfWork = table.Column<string>(nullable: true),
                    EmailAddressOfCompany = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placements", x => x.PlacementId);
                    table.ForeignKey(
                        name: "FK_Placements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Placements_StudentId",
                table: "Placements",
                column: "StudentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Placements");

            migrationBuilder.DropColumn(
                name: "EligiblityStatus",
                table: "Students");
        }
    }
}
