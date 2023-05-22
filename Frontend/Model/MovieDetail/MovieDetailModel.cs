using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.MovieDetail;
using Frontend.Service;

namespace Frontend.Model.MovieDetail;

public class MovieDetailModel :NSwagBaseClient, IMovieDetailModel

{
    private IMovieDetailClient _client;
    public MovieDetailModel(IMovieDetailClient client,IConfiguration configuration,IHttpClientFactory clientFactory):base(clientFactory,configuration)
    {
        _client = client;
    }

    public async Task<Movie?> GetMovieDetails(string movieId, string userToken)
    {
        return await _client.GetMovieDetails(movieId, userToken);
    }

    public async Task SetMovieRating(string? userToken, string movieId, int? rating = null)
    {
        await _client.SetMovieRating(userToken, movieId, rating);
    }
}