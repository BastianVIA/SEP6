using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class moveToSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeopleMovieDAO",
                columns: table => new
                {
                    MovieId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleMovieDAO", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topic = table.Column<int>(type: "int", nullable: false),
                    TimeOfActivity = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    MovieId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.MovieId);
                    table.ForeignKey(
                        name: "FK_Ratings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActedMovies",
                columns: table => new
                {
                    ActedMoviesMovieId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    DirectedMoviesMovieId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActedMoviesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => new { x.ActedMoviesId, x.ActorsId });
                    table.ForeignKey(
                        name: "FK_Actors_Movies_ActedMoviesId",
                        column: x => x.ActedMoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Actors_Persons_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    DirectedMoviesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => new { x.DirectedMoviesId, x.DirectorsId });
                    table.ForeignKey(
                        name: "FK_Directors_Movies_DirectedMoviesId",
                        column: x => x.DirectedMoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Directors_Persons_DirectorsId",
                        column: x => x.DirectorsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewRating = table.Column<int>(type: "int", nullable: true),
                    OldRating = table.Column<int>(type: "int", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialUserFollowers", x => new { x.FollowingId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_SocialUserFollowers_SocialUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocialUserFollowers_SocialUsers_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "SocialUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMovieDAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovieDAO", x => new { x.Id, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserMovieDAO_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRatingDAO",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfStars = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatingDAO", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRatingDAO_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActedMovies_ActorsId",
                table: "ActedMovies",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDAO_PostId",
                table: "ActivityDAO",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_ActorsId",
                table: "Actors",
                column: "ActorsId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectedMovies_DirectorsId",
                table: "DirectedMovies",
                column: "DirectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Directors_DirectorsId",
                table: "Directors",
                column: "DirectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Id",
                table: "Movies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_People_Id",
                table: "People",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Id",
                table: "Persons",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SocialUserFollowers_FollowerId",
                table: "SocialUserFollowers",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieDAO_UserId",
                table: "UserMovieDAO",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatingDAO_UserId",
                table: "UserRatingDAO",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActedMovies");

            migrationBuilder.DropTable(
                name: "ActivityDAO");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "DirectedMovies");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "SocialUserFollowers");

            migrationBuilder.DropTable(
                name: "UserMovieDAO");

            migrationBuilder.DropTable(
                name: "UserRatingDAO");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "PeopleMovieDAO");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "SocialUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
