using Backend.Movie.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<MovieDAO> Movies { get; set; }   

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

}