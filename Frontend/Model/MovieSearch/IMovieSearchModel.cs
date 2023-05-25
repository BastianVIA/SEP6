using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieSearch;

public interface IMovieSearchModel
{
    Task<(int NumberOfPages, List<Movie> MovieList)> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null,SortingDirection? sortingDirection = null, int? pageNumber = null);
}