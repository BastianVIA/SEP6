using Backend.Database.Transaction;
using Backend.Enum;
using Backend.Movie.Domain;
using Microsoft.EntityFrameworkCore;


namespace Backend.Movie.Infrastructure;

public class MovieRepository : IMovieRepository
{
    private const int NumberOfResultsPerPage = 10;

    public async Task<List<Domain.Movie>> SearchForMovie(string title, MovieSortingKey movieSortingKey,
        SortingDirection sortingDirection, int requestPageNumber, DbReadOnlyTransaction tx)
    {
        var query = tx.DataContext.Movies.Include(m => m.Rating)
            .Where(m => EF.Functions.Like(m.Title, $"%{title}%"));

        switch (movieSortingKey)
        {
            case MovieSortingKey.Votes:
                query = SearchForMoveOrderByVotesAsync(query, sortingDirection);
                break;
            case MovieSortingKey.ReleaseYear:
                query = SearchForMoveOrderByReleaseYearAsync(query, sortingDirection);
                break;
            case MovieSortingKey.Rating:
                query = SearchForMoveOrderByRatingAsync(query, sortingDirection);
                break;
            default:
                throw new KeyNotFoundException($"{movieSortingKey} not a valid movie sorting key ");
        }

        Task<List<MovieDAO>> foundMovies = query
            .Skip(NumberOfResultsPerPage * (requestPageNumber - 1))
            .Take(NumberOfResultsPerPage)
            .ToListAsync();


        return ToDomain(await foundMovies);
    }


    public async Task<Domain.Movie> ReadMovieFromId(string id, DbReadOnlyTransaction tx, bool includeRatings = false, bool includeActors = false, bool includeDirectors = false)
    {
        var query = tx.DataContext.Movies.Where(m => m.Id == id);
        if (includeRatings)
        {
            query = query.Include(m => m.Rating);
        }

        var result = await query.FirstOrDefaultAsync();
        if (result == null)
        {
            throw new KeyNotFoundException($"Could not find movie with id: {id}");
        }

        if (includeActors)
        {
            result.Actors = await tx.DataContext.Persons.Where(p => p.ActedMovies.Contains(result))
                .Take(NumberOfResultsPerPage)
                .ToListAsync();
        }
        if (includeDirectors)
        {
            result.Directors = await tx.DataContext.Persons.Where(p => p.DirectedMovies.Contains(result))
                .Take(NumberOfResultsPerPage)
                .ToListAsync();
        }

        return ToDomain(result);
    }

    public async Task<List<Domain.Movie>> ReadMoviesFromList(List<string> movieIds, int requestedPageNumber,
        DbReadOnlyTransaction tx)
    {
        var foundMovies = tx.DataContext.Movies.Include(m => m.Rating)
            .Where(m => movieIds.Contains(m.Id))
            .Skip(NumberOfResultsPerPage * (requestedPageNumber - 1))
            .Take(NumberOfResultsPerPage)
            .ToListAsync();
        return ToDomain(await foundMovies);
    }

    public async Task<List<Domain.Movie>> GetRecommendedMovies(int minVotes, float minRating, DbReadOnlyTransaction tx)
    {
        var random = new Random();
        var movies = tx.DataContext.Movies
            .Include(m => m.Rating)
            .Where(m => m.Rating != null && m.Rating.Votes > minVotes && m.Rating.Rating > minRating)
            .OrderBy(m => m.Rating.Votes * m.Rating.Rating)
            .Take(200)
            .ToList()
            .OrderBy(m => random.Next())
            .Take(NumberOfResultsPerPage)
            .ToList();
        return ToDomain(movies);
    }

    public async Task Update(Domain.Movie movie, DbTransaction tx)
    {
        tx.AddDomainEvents(movie.ReadAllDomainEvents());
        var movieDao = await tx.DataContext.Movies
            .Include(m => m.Rating)
            .SingleAsync(m => m.Id == movie.Id);


        excludeActorsAndDirectorsFromupdate(tx, movieDao);

        movieDao.Title = movie.Title;
        movieDao.Year = movie.ReleaseYear;

        if (movie.Rating != null)
        {
            movieDao.Rating = FromDomain(movie.Rating, movie.Id);
        }

        tx.DataContext.Movies.Update(movieDao);
    }
    public async Task<List<Domain.Movie>> GetActedMoviesForPerson(string personId, DbReadOnlyTransaction tx)
    {
        var result = await tx.DataContext.Movies
            .Include(m => m.Actors)
            .Where(m => m.Actors != null && m.Actors.Any(a => a.Id == personId)).ToListAsync();
        return ToDomain(result);
    }

    public async Task<List<Domain.Movie>> GetDirectedMoviesForPerson(string personId, DbReadOnlyTransaction tx)
    {
        var result = await tx.DataContext.Movies
            .Include(m => m.Directors)
            .Where(m => m.Directors != null && m.Directors.Any(a => a.Id == personId)).ToListAsync();
        return ToDomain(result);
        
    }

    private static void excludeActorsAndDirectorsFromupdate(DbTransaction tx, MovieDAO movieDao)
    {
        if (movieDao.Actors == null)
        {
            movieDao.Actors = new List<PersonDAO>();
        }

        if (movieDao.Directors == null)
        {
            movieDao.Directors = new List<PersonDAO>();
        }

        tx.DataContext.Entry(movieDao).Collection(m => m.Actors).IsModified = false;
        tx.DataContext.Entry(movieDao).Collection(m => m.Directors).IsModified = false;
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

    // private Domain.Person ToDomain(PersonDAO personDao)
    // {
    //     return new Person
    //     {
    //         Id = personDao.Id,
    //         Name = personDao.Name,
    //         BirthYear = personDao.BirthYear
    //     };
    // }

    private RatingDAO FromDomain(Domain.Rating rating, string movieId)
    {
        return new RatingDAO
        {
            MovieId = movieId,
            Rating = rating.AverageRating,
            Votes = rating.Votes
        };
    }

    private List<string>? ToDomain(ICollection<PersonDAO>? personDaos)
    {
        if (personDaos == null || personDaos.Count == 0)
        {
            return null;
        }

        var listOfPersons = new List<string>();
        foreach (var personDao in personDaos)
        {
            listOfPersons.Add(personDao.Id);
        }

        return listOfPersons;
    }

    private IOrderedQueryable<MovieDAO> SearchForMoveOrderByVotesAsync(IQueryable<MovieDAO> query,
        SortingDirection sortingDirection)
    {
        switch (sortingDirection)
        {
            case SortingDirection.DESC:
                return query.OrderByDescending(movie => movie.Rating != null ? movie.Rating.Votes : 0);
            case SortingDirection.ASC:
                return query.OrderBy(movie => movie.Rating != null ? movie.Rating.Votes : 0);
            default:
                throw new KeyNotFoundException($"{sortingDirection} not a valid order direction ");
        }
    }

    private IOrderedQueryable<MovieDAO> SearchForMoveOrderByRatingAsync(IQueryable<MovieDAO> query,
        SortingDirection sortingDirection)
    {
        switch (sortingDirection)
        {
            case SortingDirection.DESC:
                return query.OrderByDescending(movie => movie.Rating != null ? movie.Rating.Rating : 0);
            case SortingDirection.ASC:
                return query.OrderBy(movie => movie.Rating != null ? movie.Rating.Rating : 0);
            default:
                throw new KeyNotFoundException($"{sortingDirection} not a valid order direction ");
        }
    }


    private IOrderedQueryable<MovieDAO> SearchForMoveOrderByReleaseYearAsync(IQueryable<MovieDAO> query,
        SortingDirection sortingDirection)
    {
        switch (sortingDirection)
        {
            case SortingDirection.DESC:
                return query.OrderByDescending(movie => movie.Year);
            case SortingDirection.ASC:
                return query.OrderBy(movie => movie.Year);
            default:
                throw new KeyNotFoundException($"{sortingDirection} not a valid order direction ");
        }
    }
}