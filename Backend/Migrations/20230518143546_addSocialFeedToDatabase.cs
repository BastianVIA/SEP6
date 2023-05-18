using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addSocialFeedToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Topic = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeOfActivity = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MovieId = table.Column<string>(type: "TEXT", nullable: true),
                    NewRating = table.Column<int>(type: "INTEGER", nullable: true),
                    OldRating = table.Column<int>(type: "INTEGER", nullable: true),
                    PostId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityDAO_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_ActivityDAO_PostId",
                table: "ActivityDAO",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialUserFollowers_FollowerId",
                table: "SocialUserFollowers",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDAO");

            migrationBuilder.DropTable(
                name: "SocialUserFollowers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "SocialUsers");
        }
    }
}
