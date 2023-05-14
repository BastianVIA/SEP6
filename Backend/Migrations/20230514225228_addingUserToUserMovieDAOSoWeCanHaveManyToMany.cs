using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingUserToUserMovieDAOSoWeCanHaveManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMovieDAO",
                table: "UserMovieDAO");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserMovieDAO",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMovieDAO",
                table: "UserMovieDAO",
                columns: new[] { "Id", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMovieDAO",
                table: "UserMovieDAO");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserMovieDAO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMovieDAO",
                table: "UserMovieDAO",
                column: "Id");
        }
    }
}
