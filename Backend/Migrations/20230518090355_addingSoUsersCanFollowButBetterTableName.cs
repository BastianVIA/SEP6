using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingSoUsersCanFollowButBetterTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_FollowingId",
                table: "SocialUserDAOSocialUserDAO");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_SocialUserDAOId",
                table: "SocialUserDAOSocialUserDAO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialUserDAOSocialUserDAO",
                table: "SocialUserDAOSocialUserDAO");

            migrationBuilder.RenameTable(
                name: "SocialUserDAOSocialUserDAO",
                newName: "FollowingTable");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "FollowingTable",
                newName: "SocialUserDAO1Id");

            migrationBuilder.RenameIndex(
                name: "IX_SocialUserDAOSocialUserDAO_SocialUserDAOId",
                table: "FollowingTable",
                newName: "IX_FollowingTable_SocialUserDAOId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowingTable",
                table: "FollowingTable",
                columns: new[] { "SocialUserDAO1Id", "SocialUserDAOId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingTable_SocialUsers_SocialUserDAO1Id",
                table: "FollowingTable",
                column: "SocialUserDAO1Id",
                principalTable: "SocialUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingTable_SocialUsers_SocialUserDAOId",
                table: "FollowingTable",
                column: "SocialUserDAOId",
                principalTable: "SocialUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowingTable_SocialUsers_SocialUserDAO1Id",
                table: "FollowingTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowingTable_SocialUsers_SocialUserDAOId",
                table: "FollowingTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowingTable",
                table: "FollowingTable");

            migrationBuilder.RenameTable(
                name: "FollowingTable",
                newName: "SocialUserDAOSocialUserDAO");

            migrationBuilder.RenameColumn(
                name: "SocialUserDAO1Id",
                table: "SocialUserDAOSocialUserDAO",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowingTable_SocialUserDAOId",
                table: "SocialUserDAOSocialUserDAO",
                newName: "IX_SocialUserDAOSocialUserDAO_SocialUserDAOId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialUserDAOSocialUserDAO",
                table: "SocialUserDAOSocialUserDAO",
                columns: new[] { "FollowingId", "SocialUserDAOId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_FollowingId",
                table: "SocialUserDAOSocialUserDAO",
                column: "FollowingId",
                principalTable: "SocialUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialUserDAOSocialUserDAO_SocialUsers_SocialUserDAOId",
                table: "SocialUserDAOSocialUserDAO",
                column: "SocialUserDAOId",
                principalTable: "SocialUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
