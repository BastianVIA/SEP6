using Backend.Movie.Infrastructure.Models;
using Backend.People.Infrastructure.Models;
using Backend.Social.Infrastructure.Models;
using Backend.User.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<MovieDAO> Movies { get; set; }   
    public DbSet<RatingDAO> Ratings { get; set; }   
    public DbSet<PersonDAO> Persons { get; set; }   
    public DbSet<UserDAO> Users { get; set; }
    public DbSet<PeopleDAO> People { get; set; }
    public DbSet<PostDAO> Posts { get; set; }
    public DbSet<SocialUserDAO> SocialUsers { get; set; }

    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration, DbContextOptions options) : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
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

        modelBuilder.Entity<UserMovieDAO>()
            .HasKey(dao => new { dao.Id, dao.UserId });
        
        modelBuilder.Entity<UserMovieDAO>()
            .HasOne(um => um.User)
            .WithMany(u => u.FavoriteMovies)
            .HasForeignKey(um => um.UserId);

        modelBuilder.Entity<UserRatingDAO>()
            .HasKey(dao => new { dao.MovieId, dao.UserId });
        
        modelBuilder.Entity<UserRatingDAO>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRatings)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserReviewDAO>()
            .HasKey(dao => new {dao.MovieId, dao.UserId});

        modelBuilder.Entity<UserReviewDAO>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserReviews)
            .HasForeignKey(ur => ur.UserId);
        

        modelBuilder.Entity<PeopleMovieDAO>()
            .HasKey(dao => dao.MovieId);

        modelBuilder.Entity<PeopleMovieDAO>()
            .HasMany(pm => pm.Actors)
            .WithMany(p => p.ActedMovies)
            .UsingEntity(t => t.ToTable("ActedMovies"));
        
        modelBuilder.Entity<PeopleMovieDAO>()
            .HasMany(pm => pm.Directors)
            .WithMany(p => p.DirectedMovies)
            .UsingEntity(t => t.ToTable("DirectedMovies"));
        modelBuilder.Entity<PostDAO>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<PostDAO>()
            .HasOne(p => p.ActivityData)
            .WithOne(a => a.PostThisBelongsTo)
            .HasForeignKey<ActivityDAO>(a => a.PostId);
        

        modelBuilder.Entity<SocialUserDAO>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<SocialUserDAO>()
            .HasMany(s => s.Following)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "SocialUserFollowers",
                x => x.HasOne<SocialUserDAO>().WithMany().HasForeignKey("FollowingId"),
                x => x.HasOne<SocialUserDAO>().WithMany().HasForeignKey("FollowerId"),
                x =>
                {
                    x.HasKey("FollowingId", "FollowerId");
                    x.HasIndex("FollowerId");
                });

        modelBuilder.Entity<ReactionEntryDAO>()
            .HasKey(r => new { r.UserId, r.PostId });
    }
    
}