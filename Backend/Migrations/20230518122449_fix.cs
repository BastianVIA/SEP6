using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialUserDAOId",
                table: "SocialUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialUsers_SocialUserDAOId",
                table: "SocialUsers",
                column: "SocialUserDAOId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialUsers_SocialUsers_SocialUserDAOId",
                table: "SocialUsers",
                column: "SocialUserDAOId",
                principalTable: "SocialUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialUsers_SocialUsers_SocialUserDAOId",
                table: "SocialUsers");

            migrationBuilder.DropIndex(
                name: "IX_SocialUsers_SocialUserDAOId",
                table: "SocialUsers");

            migrationBuilder.DropColumn(
                name: "SocialUserDAOId",
                table: "SocialUsers");
        }
    }
}
