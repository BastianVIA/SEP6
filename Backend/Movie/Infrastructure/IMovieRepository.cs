using Backend.Enum;

namespace Backend.Movie.Infrastructure;

public interface IMovieRepository
{
    Task<List<Domain.Movie>> SearchForMovie(string title, MovieSortingKey movieSortingKey,
        SortingDirection sortingDirection, int requestPageNumber);
    Task<Domain.Movie> ReadMovieFromId(string id);
    Task<List<Domain.Movie>> ReadMoviesFromList(List<string> movieIds, int requestedPageNumber);
    Task<List<Domain.Movie>> GetRecommendedMovies(int minVotes, float minRating);

}