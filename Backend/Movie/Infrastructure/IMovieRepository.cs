using Backend.Database.Transaction;
using Backend.Enum;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Movie.Infrastructure;

public interface IMovieRepository
{
    Task<(List<Domain.Movie> Movies, int NumberOfPages)> SearchForMovieAsync(string title, MovieSortingKey movieSortingKey,
        SortingDirection sortingDirection, int requestPageNumber, DbReadOnlyTransaction tx);

    Task<Domain.Movie> ReadMovieFromIdAsync(string id, DbReadOnlyTransaction tx, bool includeRatings = false,
        bool includeActors = false, bool includeDirectors = false);
    Task<List<Domain.Movie>> ReadMoviesFromListAsync(List<string> movieIds, int requestedPageNumber, DbReadOnlyTransaction tx);
    Task<List<Domain.Movie>> GetRecommendedMoviesAsync(int minVotes, float minRating, DbReadOnlyTransaction tx);
    Task Update(Domain.Movie movie, DbTransaction tx);

    Task<List<Domain.Movie>> GetTopMoviesAsync(int MinVotesBeforeTop100, int pageNumber,DbReadOnlyTransaction transaction);
    Task<List<Domain.Movie>?> GetActedMoviesForPersonAsync(string personId, DbReadOnlyTransaction tx);
    Task<List<Domain.Movie>?> GetDirectedMoviesForPersonAsync(string personId, DbReadOnlyTransaction tx);

    Task<int> NumberOfResultsForSearch(string title, DbReadOnlyTransaction tx);
}