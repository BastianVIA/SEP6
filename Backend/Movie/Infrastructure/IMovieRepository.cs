using Backend.Database.Transaction;
using Backend.Enum;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Movie.Infrastructure;

public interface IMovieRepository
{
    Task<List<Domain.Movie>> SearchForMovie(string title, MovieSortingKey movieSortingKey,
        SortingDirection sortingDirection, int requestPageNumber, DbReadOnlyTransaction tx);
    Task<Domain.Movie> ReadMovieFromId(string id, DbReadOnlyTransaction tx);
    Task<List<Domain.Movie>> ReadMoviesFromList(List<string> movieIds, int requestedPageNumber, DbReadOnlyTransaction tx);
    Task<List<Domain.Movie>> GetRecommendedMovies(int minVotes, float minRating, DbReadOnlyTransaction tx);
    Task Update(Domain.Movie movie, DbTransaction tx);

}