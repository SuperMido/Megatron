using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Megatron.Data.Migrations
{
    public partial class AddMessageToCommentArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "CommentArticles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CommentArticles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "CommentArticles");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "CommentArticles");
        }
    }
}
