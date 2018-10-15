using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ApplicationCore.Migrations
{
    public partial class SubscribeArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscribeArticleId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscribeArticles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    SubscribeAt = table.Column<DateTime>(nullable: false),
                    Token = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribeArticles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscribeArticleId",
                table: "AspNetUsers",
                column: "SubscribeArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscribeArticles_Email",
                table: "SubscribeArticles",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscribeArticles_Token",
                table: "SubscribeArticles",
                column: "Token",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SubscribeArticles_SubscribeArticleId",
                table: "AspNetUsers",
                column: "SubscribeArticleId",
                principalTable: "SubscribeArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SubscribeArticles_SubscribeArticleId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubscribeArticles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscribeArticleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscribeArticleId",
                table: "AspNetUsers");
        }
    }
}
