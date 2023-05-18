﻿// <auto-generated />
using System;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230518123349_fixSocialUserTable")]
    partial class fixSocialUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Backend.Movie.Infrastructure.MovieDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Title");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.PersonDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int?>("BirthYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Backend.Movie.Infrastructure.RatingDAO", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("TEXT");

                    b.Property<float>("Rating")
                        .HasColumnType("REAL");

                    b.Property<int>("Votes")
                        .HasColumnType("INTEGER");

                    b.HasKey("MovieId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.ActivityDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("MovieId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NewRating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("ActivityDAO");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.PostDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeOfActivity")
                        .HasColumnType("TEXT");

                    b.Property<int>("Topic")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.SocialUserDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SocialUsers");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserMovieDAO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMovieDAO");
                });

            modelBuilder.Entity("Backend.User.Infrastructure.UserRatingDAO", b =>
                {
                    b.Property<string>("MovieId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfStars")
                        .HasColumnType("INTEGER");

                    b.HasKey("MovieId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRatingDAO");
                });

            modelBuilder.Entity("MovieDAOPersonDAO", b =>
                {
                    b.Property<string>("ActedMoviesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActorsId")
                        .HasColumnType("TEXT");

                    b.HasKey("ActedMoviesId", "ActorsId");

                    b.HasIndex("ActorsId");

                    b.ToTable("Actors", (string)null);
                });

            modelBuilder.Entity("MovieDAOPersonDAO1", b =>
                {
                    b.Property<string>("DirectedMoviesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DirectorsId")
                        .HasColumnType("TEXT");

                    b.HasKey("DirectedMoviesId", "DirectorsId");

                    b.HasIndex("DirectorsId");

                    b.ToTable("Directors", (string)null);
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

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.SocialUserDAO", b =>
                {
                    b.HasOne("Backend.SocialFeed.Infrastructure.SocialUserDAO", null)
                        .WithMany("Following")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
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

            modelBuilder.Entity("Backend.Movie.Infrastructure.MovieDAO", b =>
                {
                    b.Navigation("Rating");
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.PostDAO", b =>
                {
                    b.Navigation("ActivityData")
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.SocialFeed.Infrastructure.SocialUserDAO", b =>
                {
                    b.Navigation("Following");
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
