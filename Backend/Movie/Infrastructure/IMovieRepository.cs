using Backend.Enum;

namespace Backend.Movie.Infrastructure;

public interface IMovieRepository
{
    Task<List<Domain.Movie>> SearchForMovie(string title, MovieSortingKey movieSortingKey, SortingDirection orderDirection);
    Task<Domain.Movie> ReadMovieFromId(string id);

}