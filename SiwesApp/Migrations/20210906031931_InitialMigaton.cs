using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class InitialMigaton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    RoleName = table.Column<string>(nullable: true),
                    RoleDescription = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    UserTypeId = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ShortToken = table.Column<string>(nullable: true),
                    LongToken = table.Column<string>(nullable: true),
                    LastLoginDateTime = table.Column<DateTimeOffset>(nullable: true),
                    SecondToLastLoginDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndustrialSupervisors",
                columns: table => new
                {
                    IndustrialSupervisorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    SectionOfWork = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustrialSupervisors", x => x.IndustrialSupervisorId);
                    table.ForeignKey(
                        name: "FK_IndustrialSupervisors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    LecturerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.LecturerId);
                    table.ForeignKey(
                        name: "FK_Lecturers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiwesAdmins",
                columns: table => new
                {
                    SiwesAdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiwesAdmins", x => x.SiwesAdminId);
                    table.ForeignKey(
                        name: "FK_SiwesAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiwesCoordinators",
                columns: table => new
                {
                    SiwesCoordinatorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiwesCoordinators", x => x.SiwesCoordinatorId);
                    table.ForeignKey(
                        name: "FK_SiwesCoordinators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignStudentToLecturers",
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
                    table.PrimaryKey("PK_AssignStudentToLecturers", x => x.AssignStudentToLecturerId);
                    table.ForeignKey(
                        name: "FK_AssignStudentToLecturers_IndustrialSupervisors_IndustrialSupervisorId",
                        column: x => x.IndustrialSupervisorId,
                        principalTable: "IndustrialSupervisors",
                        principalColumn: "IndustrialSupervisorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignStudentToLecturers_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    LogBookId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MatricNumber = table.Column<int>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    EligiblityStatus = table.Column<int>(nullable: true),
                    DOB = table.Column<DateTimeOffset>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    LGA = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PlacementId = table.Column<int>(nullable: true),
                    AssignStudentToLecturerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_AssignStudentToLecturers_AssignStudentToLecturerId",
                        column: x => x.AssignStudentToLecturerId,
                        principalTable: "AssignStudentToLecturers",
                        principalColumn: "AssignStudentToLecturerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogBooks",
                columns: table => new
                {
                    LogBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    Day = table.Column<DateTimeOffset>(nullable: false),
                    Time = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentterId = table.Column<int>(nullable: false),
                    CommentDetail = table.Column<string>(nullable: true),
                    LogBookId = table.Column<int>(nullable: false),
                    LecturerId = table.Column<int>(nullable: true),
                    IndustrialSupervisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_IndustrialSupervisors_IndustrialSupervisorId",
                        column: x => x.IndustrialSupervisorId,
                        principalTable: "IndustrialSupervisors",
                        principalColumn: "IndustrialSupervisorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_LogBooks_LogBookId",
                        column: x => x.LogBookId,
                        principalTable: "LogBooks",
                        principalColumn: "LogBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraderId = table.Column<int>(nullable: false),
                    ObtainedGrade = table.Column<int>(nullable: false),
                    ObtainableGrade = table.Column<int>(nullable: false),
                    LogBookId = table.Column<int>(nullable: false),
                    LecturerId = table.Column<int>(nullable: true),
                    IndustrialSupervisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                    table.ForeignKey(
                        name: "FK_Grades_IndustrialSupervisors_IndustrialSupervisorId",
                        column: x => x.IndustrialSupervisorId,
                        principalTable: "IndustrialSupervisors",
                        principalColumn: "IndustrialSupervisorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_LogBooks_LogBookId",
                        column: x => x.LogBookId,
                        principalTable: "LogBooks",
                        principalColumn: "LogBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssignStudentToLecturers_IndustrialSupervisorId",
                table: "AssignStudentToLecturers",
                column: "IndustrialSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignStudentToLecturers_LecturerId",
                table: "AssignStudentToLecturers",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IndustrialSupervisorId",
                table: "Comments",
                column: "IndustrialSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LecturerId",
                table: "Comments",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LogBookId",
                table: "Comments",
                column: "LogBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_IndustrialSupervisorId",
                table: "Grades",
                column: "IndustrialSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_LecturerId",
                table: "Grades",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_LogBookId",
                table: "Grades",
                column: "LogBookId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustrialSupervisors_UserId",
                table: "IndustrialSupervisors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_UserId",
                table: "Lecturers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogBooks_StudentId",
                table: "LogBooks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_StudentId",
                table: "Placements",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiwesAdmins_UserId",
                table: "SiwesAdmins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiwesCoordinators_UserId",
                table: "SiwesCoordinators",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AssignStudentToLecturerId",
                table: "Students",
                column: "AssignStudentToLecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Placements");

            migrationBuilder.DropTable(
                name: "SiwesAdmins");

            migrationBuilder.DropTable(
                name: "SiwesCoordinators");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "LogBooks");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "AssignStudentToLecturers");

            migrationBuilder.DropTable(
                name: "IndustrialSupervisors");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
