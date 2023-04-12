using Frontend.Entities;

namespace Frontend.Model.MovieSearchModel;

public interface IMovieSearchModel
{
    Task<IList<Movie>> SearchForMovieAsync(string title);
}