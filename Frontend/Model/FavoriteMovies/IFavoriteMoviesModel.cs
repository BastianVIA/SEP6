using Frontend.Entities;

namespace Frontend.Model.FavoriteMovies;

public interface IFavoriteMoviesModel
{
    Task<IList<Movie>> GetFavoriteMovies(string userToken);
    Task AddToFavoriteMovies(string bearerToken, string movieId);
}