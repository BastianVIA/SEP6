using Frontend.Entities;

namespace Frontend.Network.MovieDetail;

public interface IMovieDetailClient
{
    public Task<Movie?> GetMovieDetails(string movieId);

}