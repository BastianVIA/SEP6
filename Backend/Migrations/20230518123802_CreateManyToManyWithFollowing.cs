using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateManyToManyWithFollowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialUserFollowers",
                columns: table => new
                {
                    FollowingId = table.Column<string>(type: "TEXT", nullable: false),
                    FollowerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialUserFollowers", x => new { x.FollowingId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_SocialUserFollowers_SocialUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialUserFollowers_SocialUsers_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialUserFollowers_FollowerId",
                table: "SocialUserFollowers",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialUserFollowers");
        }
    }
}
