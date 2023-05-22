using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingMigrationForReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ReviewBody",
                table: "ActivityDAO",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserReviewDAO",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    MovieId = table.Column<string>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "IX_UserReviewDAO_UserId",
                table: "UserReviewDAO",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReviewDAO");
            
            migrationBuilder.DropColumn(
                name: "ReviewBody",
                table: "ActivityDAO");
            
        }
    }
}
