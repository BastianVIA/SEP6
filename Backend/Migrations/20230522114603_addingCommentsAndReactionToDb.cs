using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingCommentsAndReactionToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Contents = table.Column<string>(type: "TEXT", nullable: false),
                    PostDAOId = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "ReactionDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfReaction = table.Column<int>(type: "INTEGER", nullable: false),
                    PostDAOId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionDAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionDAO_Posts_PostDAOId",
                        column: x => x.PostDAOId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentDAO_PostDAOId",
                table: "CommentDAO",
                column: "PostDAOId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionDAO_PostDAOId",
                table: "ReactionDAO",
                column: "PostDAOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentDAO");

            migrationBuilder.DropTable(
                name: "ReactionDAO");
        }
    }
}
