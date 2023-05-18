using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingSoUsersCanFollow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialUserDAOSocialUserDAO",
                columns: table => new
                {
                    FollowingId = table.Column<string>(type: "TEXT", nullable: false),
                    SocialUserDAOId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialUserDAOSocialUserDAO", x => new { x.FollowingId, x.SocialUserDAOId });
                    table.ForeignKey(
                        name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_SocialUserDAOId",
                        column: x => x.SocialUserDAOId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialUserDAOSocialUserDAO_SocialUserDAOId",
                table: "SocialUserDAOSocialUserDAO",
                column: "SocialUserDAOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialUserDAOSocialUserDAO");
        }
    }
}
