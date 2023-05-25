using Frontend.Entities;

namespace Frontend.Network.FavoriteMovies;

public interface IFavoriteMoviesClient
{
    public Task<IList<Movie>> GetFavoriteMovies(string UID);
    public Task AddToFavoriteMovies(string bearerToken, string movieId);

}