using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieDAO_Users_UserDAOId",
                table: "UserMovieDAO");

            migrationBuilder.DropIndex(
                name: "IX_UserMovieDAO_UserDAOId",
                table: "UserMovieDAO");

            migrationBuilder.DropColumn(
                name: "UserDAOId",
                table: "UserMovieDAO");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieDAO_UserId",
                table: "UserMovieDAO",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieDAO_Users_UserId",
                table: "UserMovieDAO",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieDAO_Users_UserId",
                table: "UserMovieDAO");

            migrationBuilder.DropIndex(
                name: "IX_UserMovieDAO_UserId",
                table: "UserMovieDAO");

            migrationBuilder.AddColumn<string>(
                name: "UserDAOId",
                table: "UserMovieDAO",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieDAO_UserDAOId",
                table: "UserMovieDAO",
                column: "UserDAOId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieDAO_Users_UserDAOId",
                table: "UserMovieDAO",
                column: "UserDAOId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
