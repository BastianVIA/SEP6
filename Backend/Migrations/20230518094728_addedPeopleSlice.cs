using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedPeopleSlice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BirthYear = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeopleMovieDAO",
                columns: table => new
                {
                    MovieId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleMovieDAO", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "ActedMovies",
                columns: table => new
                {
                    ActedMoviesMovieId = table.Column<string>(type: "TEXT", nullable: false),
                    ActorsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActedMovies", x => new { x.ActedMoviesMovieId, x.ActorsId });
                    table.ForeignKey(
                        name: "FK_ActedMovies_PeopleMovieDAO_ActedMoviesMovieId",
                        column: x => x.ActedMoviesMovieId,
                        principalTable: "PeopleMovieDAO",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActedMovies_People_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectedMovies",
                columns: table => new
                {
                    DirectedMoviesMovieId = table.Column<string>(type: "TEXT", nullable: false),
                    DirectorsId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectedMovies", x => new { x.DirectedMoviesMovieId, x.DirectorsId });
                    table.ForeignKey(
                        name: "FK_DirectedMovies_PeopleMovieDAO_DirectedMoviesMovieId",
                        column: x => x.DirectedMoviesMovieId,
                        principalTable: "PeopleMovieDAO",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectedMovies_People_DirectorsId",
                        column: x => x.DirectorsId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActedMovies_ActorsId",
                table: "ActedMovies",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectedMovies_DirectorsId",
                table: "DirectedMovies",
                column: "DirectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_People_Id",
                table: "People",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActedMovies");

            migrationBuilder.DropTable(
                name: "DirectedMovies");

            migrationBuilder.DropTable(
                name: "PeopleMovieDAO");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
