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
    [Migration("20230426153510_removedNullable")]
    partial class removedNullable
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
#pragma warning restore 612, 618
        }
    }
}
