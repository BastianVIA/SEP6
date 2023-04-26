﻿using Backend.Movie.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<MovieDAO> Movies { get; set; }   
    public DbSet<RatingDAO> Ratings { get; set; }   
    public DbSet<PersonDAO> Persons { get; set; }   

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieDAO>()
            .HasOne(m => m.Rating)
            .WithOne()
            .HasForeignKey<RatingDAO>(r => r.MovieId);
        
        modelBuilder.Entity<MovieDAO>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.ActedMovies)
            .UsingEntity(j => j.ToTable("Actors"));
     
        modelBuilder.Entity<MovieDAO>()
            .HasMany(m => m.Directors)
            .WithMany(d => d.DirectedMovies)
            .UsingEntity(j => j.ToTable("Directors"));
        
    }
    
}