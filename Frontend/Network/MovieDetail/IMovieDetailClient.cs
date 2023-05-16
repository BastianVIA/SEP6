using Frontend.Entities;

namespace Frontend.Network.MovieDetail;

public interface IMovieDetailClient
{
    public Task<Movie?> GetMovieDetails(string movieId, string? userToken);
    public Task SetMovieRating(string? userToken, string movieId, int? rating = null);

}