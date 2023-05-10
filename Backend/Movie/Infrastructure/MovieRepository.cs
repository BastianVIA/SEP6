using Backend.Database;
using Backend.Enum;
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

    public async Task<List<Domain.Movie>> SearchForMovie(string title, MovieSortingKey movieSortingKey,
        SortingDirection orderDirection)
    {
        Task<List<MovieDAO>> foundMovies;

        switch (movieSortingKey)
        {
            case MovieSortingKey.Votes:
                foundMovies = SearchForMoveOrderByVotesAsync(title, orderDirection);
                break;
            case MovieSortingKey.ReleaseYear:
                foundMovies = SearchForMoveOrderByReleaseYearAsync(title, orderDirection);
                break;
            default:
                throw new KeyNotFoundException($"{movieSortingKey} not a valid movie sorting key ");
        }


        return ToDomain(await foundMovies);
    }

    private Task<List<MovieDAO>> SearchForMoveOrderByVotesAsync(string title, SortingDirection orderDirection)
    {
        switch (orderDirection)
        {
            case SortingDirection.DESC:
                return _database.Movies.Include(m => m.Rating)
                    .Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
                    .OrderByDescending(movie => movie.Rating != null ? movie.Rating.Votes : 0)
                    .Take(NumberOfResults).ToListAsync();
            case SortingDirection.ASC:
                return _database.Movies.Include(m => m.Rating)
                    .Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
                    .OrderBy(movie => movie.Rating != null ? movie.Rating.Votes : 0)
                    .Take(NumberOfResults).ToListAsync(); 
            default:
                throw new KeyNotFoundException($"{orderDirection} not a valid order direction ");
        }
    }

    private Task<List<MovieDAO>> SearchForMoveOrderByReleaseYearAsync(string title, SortingDirection orderDirection)
    {
        switch (orderDirection)
        {
            case SortingDirection.DESC:
                return                  _database.Movies.Include(m => m.Rating)
                    .Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
                    .OrderByDescending(movie => movie.Year)
                    .Take(NumberOfResults).ToListAsync();
            case SortingDirection.ASC:
                return _database.Movies.Include(m => m.Rating)
                    .Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
                    .OrderBy(movie => movie.Year)
                    .Take(NumberOfResults).ToListAsync(); 
            default:
                throw new KeyNotFoundException($"{orderDirection} not a valid order direction ");
        }
    }



    public async Task<Domain.Movie> ReadMovieFromId(string id)
    {
        var result = await _database.Movies.Where(m => m.Id == id).Include(m => m.Rating).FirstOrDefaultAsync();

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