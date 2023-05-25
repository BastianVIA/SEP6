using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class migrationsForSQLserver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeMovieWasAdded",
                table: "UserMovieDAO",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewBody",
                table: "ActivityDAO",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommentDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostDAOId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentDAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentDAO_Posts_PostDAOId",
                        column: x => x.PostDAOId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReactionEntryDAO",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TypeOfReaction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionEntryDAO", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_ReactionEntryDAO_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReviewDAO",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviewDAO", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserReviewDAO_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentDAO_PostDAOId",
                table: "CommentDAO",
                column: "PostDAOId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionEntryDAO_PostId",
                table: "ReactionEntryDAO",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviewDAO_UserId",
                table: "UserReviewDAO",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentDAO");

            migrationBuilder.DropTable(
                name: "ReactionEntryDAO");

            migrationBuilder.DropTable(
                name: "UserReviewDAO");

            migrationBuilder.DropColumn(
                name: "TimeMovieWasAdded",
                table: "UserMovieDAO");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ReviewBody",
                table: "ActivityDAO");
        }
    }
}
