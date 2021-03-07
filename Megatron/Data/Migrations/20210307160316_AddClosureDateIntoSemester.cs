using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Megatron.Data.Migrations
{
    public partial class AddClosureDateIntoSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SemesterClosureDate",
                table: "Semesters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SemesterClosureDate",
                table: "Semesters");
        }
    }
}