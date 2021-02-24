using Microsoft.EntityFrameworkCore.Migrations;

namespace Megatron.Data.Migrations
{
    public partial class AddStatusMessageIntoArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusMessage",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusMessage",
                table: "Articles");
        }
    }
}
