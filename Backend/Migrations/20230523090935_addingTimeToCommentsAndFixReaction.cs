using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addingTimeToCommentsAndFixReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionEntryDAO_Posts_PostDAOId",
                table: "ReactionEntryDAO");

            migrationBuilder.DropIndex(
                name: "IX_ReactionEntryDAO_PostDAOId",
                table: "ReactionEntryDAO");

            migrationBuilder.DropColumn(
                name: "PostDAOId",
                table: "ReactionEntryDAO");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "CommentDAO",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ReactionEntryDAO_PostId",
                table: "ReactionEntryDAO",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionEntryDAO_Posts_PostId",
                table: "ReactionEntryDAO",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionEntryDAO_Posts_PostId",
                table: "ReactionEntryDAO");

            migrationBuilder.DropIndex(
                name: "IX_ReactionEntryDAO_PostId",
                table: "ReactionEntryDAO");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "CommentDAO");

            migrationBuilder.AddColumn<string>(
                name: "PostDAOId",
                table: "ReactionEntryDAO",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReactionEntryDAO_PostDAOId",
                table: "ReactionEntryDAO",
                column: "PostDAOId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionEntryDAO_Posts_PostDAOId",
                table: "ReactionEntryDAO",
                column: "PostDAOId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
