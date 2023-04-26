using Frontend.Entities;

namespace Frontend.Model.MovieDetailModel;

public interface IMovieDetailModel
{
    Task<Movie> GetMovieDetails(string movieId);
}