﻿// <auto-generated />
using System;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230523145117_moveToSqlServer")]
    partial class moveToSqlServer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Movie.Infrastructure.MovieDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Title");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.PersonDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.RatingDAO", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("Votes")
                        .HasColumnType("int");

                    b.HasKey("MovieId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Backend.People.Infrastructure.PeopleDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BirthYear")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Backend.People.Infrastructure.PeopleMovieDAO", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MovieId");

                    b.ToTable("PeopleMovieDAO");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.ActivityDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NewRating")
                        .HasColumnType("int");

                    b.Property<int?>("OldRating")
                        .HasColumnType("int");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("ActivityDAO");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.PostDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TimeOfActivity")
                        .HasColumnType("datetime2");

                    b.Property<int>("Topic")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.SocialUserDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("SocialUsers");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserMovieDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMovieDAO");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserRatingDAO", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumberOfStars")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRatingDAO");
                });

            modelBuilder.Entity("MovieDAOPersonDAO", b =>
                {
                    b.Property<string>("ActedMoviesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ActedMoviesId", "ActorsId");

                    b.HasIndex("ActorsId");

                    b.ToTable("Actors", (string)null);
                });

            modelBuilder.Entity("MovieDAOPersonDAO1", b =>
                {
                    b.Property<string>("DirectedMoviesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DirectorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DirectedMoviesId", "DirectorsId");

                    b.HasIndex("DirectorsId");

                    b.ToTable("Directors", (string)null);
                });

            modelBuilder.Entity("PeopleDAOPeopleMovieDAO", b =>
                {
                    b.Property<string>("ActedMoviesMovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ActedMoviesMovieId", "ActorsId");

                    b.HasIndex("ActorsId");

                    b.ToTable("ActedMovies", (string)null);
                });

            modelBuilder.Entity("PeopleDAOPeopleMovieDAO1", b =>
                {
                    b.Property<string>("DirectedMoviesMovieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DirectorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DirectedMoviesMovieId", "DirectorsId");

                    b.HasIndex("DirectorsId");

                    b.ToTable("DirectedMovies", (string)null);
                });

            modelBuilder.Entity("SocialUserFollowers", b =>
                {
                    b.Property<string>("FollowingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FollowerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FollowingId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("SocialUserFollowers");
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.RatingDAO", b =>
                {
                    b.HasOne("Backend.Movie.Infrastructure.MovieDAO", null)
                        .WithOne("Rating")
                        .HasForeignKey("Backend.Movie.Infrastructure.RatingDAO", "MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.ActivityDAO", b =>
                {
                    b.HasOne("Backend.SocialFeed.Infrastructure.PostDAO", "PostThisBelongsTo")
                        .WithOne("ActivityData")
                        .HasForeignKey("Backend.SocialFeed.Infrastructure.ActivityDAO", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostThisBelongsTo");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserMovieDAO", b =>
                {
                    b.HasOne("Backend.User.Infrastructure.UserDAO", "User")
                        .WithMany("FavoriteMovies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserRatingDAO", b =>
                {
                    b.HasOne("Backend.User.Infrastructure.UserDAO", "User")
                        .WithMany("UserRatings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MovieDAOPersonDAO", b =>
                {
                    b.HasOne("Backend.Movie.Infrastructure.MovieDAO", null)
                        .WithMany()
                        .HasForeignKey("ActedMoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Movie.Infrastructure.PersonDAO", null)
                        .WithMany()
                        .HasForeignKey("ActorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieDAOPersonDAO1", b =>
                {
                    b.HasOne("Backend.Movie.Infrastructure.MovieDAO", null)
                        .WithMany()
                        .HasForeignKey("DirectedMoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Movie.Infrastructure.PersonDAO", null)
                        .WithMany()
                        .HasForeignKey("DirectorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PeopleDAOPeopleMovieDAO", b =>
                {
                    b.HasOne("Backend.People.Infrastructure.PeopleMovieDAO", null)
                        .WithMany()
                        .HasForeignKey("ActedMoviesMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.People.Infrastructure.PeopleDAO", null)
                        .WithMany()
                        .HasForeignKey("ActorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PeopleDAOPeopleMovieDAO1", b =>
                {
                    b.HasOne("Backend.People.Infrastructure.PeopleMovieDAO", null)
                        .WithMany()
                        .HasForeignKey("DirectedMoviesMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.People.Infrastructure.PeopleDAO", null)
                        .WithMany()
                        .HasForeignKey("DirectorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialUserFollowers", b =>
                {
                    b.HasOne("Backend.SocialFeed.Infrastructure.SocialUserDAO", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Backend.SocialFeed.Infrastructure.SocialUserDAO", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.MovieDAO", b =>
                {
                    b.Navigation("Rating");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.PostDAO", b =>
                {
                    b.Navigation("ActivityData");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserDAO", b =>
                {
                    b.Navigation("FavoriteMovies");

                    b.Navigation("UserRatings");
                });
#pragma warning restore 612, 618
        }
    }
}