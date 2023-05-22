using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieSearch;

public interface IMovieSearchModel
{
    Task<List<Movie>> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null,SortingDirection? sortingDirection = null, int? pageNumber = null);
}