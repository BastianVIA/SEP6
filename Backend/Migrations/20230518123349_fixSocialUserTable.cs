using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixSocialUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialUsers_SocialUsers_SocialUserDAOId",
                table: "SocialUsers");

            migrationBuilder.DropTable(
                name: "FollowingTable");

            migrationBuilder.DropIndex(
                name: "IX_SocialUsers_SocialUserDAOId",
                table: "SocialUsers");

            migrationBuilder.DropColumn(
                name: "SocialUserDAOId",
                table: "SocialUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialUserDAOId",
                table: "SocialUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FollowingTable",
                columns: table => new
                {
                    SocialUserDAO1Id = table.Column<string>(type: "TEXT", nullable: false),
                    SocialUserDAOId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowingTable", x => new { x.SocialUserDAO1Id, x.SocialUserDAOId });
                    table.ForeignKey(
                        name: "FK_FollowingTable_SocialUsers_SocialUserDAO1Id",
                        column: x => x.SocialUserDAO1Id,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowingTable_SocialUsers_SocialUserDAOId",
                        column: x => x.SocialUserDAOId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialUsers_SocialUserDAOId",
                table: "SocialUsers",
                column: "SocialUserDAOId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowingTable_SocialUserDAOId",
                table: "FollowingTable",
                column: "SocialUserDAOId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialUsers_SocialUsers_SocialUserDAOId",
                table: "SocialUsers",
                column: "SocialUserDAOId",
                principalTable: "SocialUsers",
                principalColumn: "Id");
        }
    }
}
