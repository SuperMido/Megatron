using Microsoft.EntityFrameworkCore.Migrations;

namespace Megatron.Data.Migrations
{
    public partial class AddArticleIntoDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ArticleDocuments_ArticleId",
                table: "ArticleDocuments",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleDocuments_Articles_ArticleId",
                table: "ArticleDocuments",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleDocuments_Articles_ArticleId",
                table: "ArticleDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ArticleDocuments_ArticleId",
                table: "ArticleDocuments");
        }
    }
}
