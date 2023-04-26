using Backend.Database;
using Backend.Movie.Domain;
using Backend.Service;
using Microsoft.AspNetCore.Html;
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
        var foundMovies = _database.Movies.Include(m => m.Rating).Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
            .Take(NumberOfResults).ToListAsync();

        return ToDomain(await foundMovies);
    }


    public async Task<Domain.Movie> ReadMovieFromId(string id)
    {

        var result = await  _database.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();
        
        if (result == null)
        {
            throw new KeyNotFoundException($"Could not find movie with id: {id}");
        }
        result.Actors = await _database.Persons.Where(p => p.ActedMovies.Contains(result)).Take(NumberOfResults)
            .ToListAsync();
        result.Directors = await _database.Persons.Where(p => p.DirectedMovies.Contains(result)).Take(NumberOfResults)
            .ToListAsync();
        
        return ToDomain(result);
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
            ReleaseYear = movieDao.Year,
            Rating = ToDomain(movieDao.Rating),
            Actors = ToDomain(movieDao.Actors),
            Directors = ToDomain(movieDao.Directors)
        };
    }

    private Domain.Rating? ToDomain(RatingDAO? ratingDao)
    {
        if (ratingDao == null)
        {
            return null;
        }

        return new Domain.Rating
        {
            AverageRating = ratingDao.Rating,
            Votes = ratingDao.Votes
        };
    }

    private Domain.Person ToDomain(PersonDAO personDao)
    {
        return new Person
        {
            Id = personDao.Id,
            Name = personDao.Name,
            BirthYear = personDao.BirthYear
        };
    }

    private List<Domain.Person>? ToDomain(ICollection<PersonDAO>? personDaos)
    {
        if (personDaos == null || personDaos.Count == 0)
        {
            return null;
        }

        var listOfPersons = new List<Domain.Person>();
        foreach (var personDao in personDaos)
        {
            listOfPersons.Add(ToDomain(personDao));
        }

        return listOfPersons;
    }
}