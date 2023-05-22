using Frontend.Entities;

namespace Frontend.Model.MovieDetail;

public interface IMovieDetailModel
{
    public Task<Movie?> GetMovieDetails(string movieId, string userToken);
    public Task SetMovieRating(string? userToken, string movieId, int? rating = null);
    
}