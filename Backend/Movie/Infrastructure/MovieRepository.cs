using Backend.Database;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

namespace Backend.Movie.Infrastructure;

public class MovieRepository : IMovieRepository
{
    private readonly DataContext _database;
    private const int NumberOfResults = 10;

    public MovieRepository(DataContext database)
    {
        _database = database;
    }

    public async Task<List<Domain.Movie>> SearchForMovie(string title)
    {
        var foundMovies = _database.Movies.Where(m => EF.Functions.Like(m.Title, $"%{title}%")).Take(NumberOfResults).ToListAsync();
        
        return ToDomain(await foundMovies);
    }

    private List<Domain.Movie> ToDomain(List<MovieDAO> movieDaos)
    {
        var listOfDomainMovies = new List<Domain.Movie>();
        foreach (var movieDao in movieDaos)
        {
            listOfDomainMovies.Add(ToDomain(movieDao));
        }

        return listOfDomainMovies;
    }

    private Domain.Movie ToDomain(MovieDAO movieDao)
    {
        return new Domain.Movie
        {
            Id = movieDao.Id,
            Title = movieDao.Title,
            ReleaseYear = movieDao.Year
        };
    }
}