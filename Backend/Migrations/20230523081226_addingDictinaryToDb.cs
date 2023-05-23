using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingDictinaryToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionDAO");

            migrationBuilder.CreateTable(
                name: "ReactionEntryDAO",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    PostId = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfReaction = table.Column<int>(type: "INTEGER", nullable: false),
                    PostDAOId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionEntryDAO", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_ReactionEntryDAO_Posts_PostDAOId",
                        column: x => x.PostDAOId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactionEntryDAO_PostDAOId",
                table: "ReactionEntryDAO",
                column: "PostDAOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionEntryDAO");

            migrationBuilder.CreateTable(
                name: "ReactionDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PostDAOId = table.Column<string>(type: "TEXT", nullable: true),
                    TypeOfReaction = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "IX_ReactionDAO_PostDAOId",
                table: "ReactionDAO",
                column: "PostDAOId");
        }
    }
}
