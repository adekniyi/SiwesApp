using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiwesApp.Migrations
{
    public partial class ChaingTimeotring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "LogBooks",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Time",
                table: "LogBooks",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
