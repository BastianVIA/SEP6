using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.MovieSearch;

public interface IMovieSearchClient
{
    public Task<(int NumberOfPages, List<Movie> MovieList)> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null,
        SortingDirection? sortingDirection = null, int? pageNumber = null);
}