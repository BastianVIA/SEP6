using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.FavoriteMovies;
using Frontend.Service;


namespace Frontend.Model.FavoriteMovies;

public class FavoriteMoviesModel : NSwagBaseClient, IFavoriteMoviesModel
{

    private IFavoriteMoviesClient _client;

    public FavoriteMoviesModel(IFavoriteMoviesClient client)
    {
        _client = client;
    }

    public async Task<IList<Movie>> GetFavoriteMovies(string userToken = null, string UID = null)
    {
        return await _client.GetFavoriteMovies(userToken, UID);
    }

    public async Task AddToFavoriteMovies(string bearerToken, string movieId)
    {
        await _client.AddToFavoriteMovies(bearerToken, movieId);
    }
}