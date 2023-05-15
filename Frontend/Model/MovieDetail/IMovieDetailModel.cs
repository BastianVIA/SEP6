using Frontend.Entities;

namespace Frontend.Model.MovieDetail;

public interface IMovieDetailModel
{
    Task<Movie> GetMovieDetails(string movieId);
}