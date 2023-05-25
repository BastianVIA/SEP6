using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Events;
using Frontend.Network;
using Frontend.Network.FavoriteMovies;
using Frontend.Service;


namespace Frontend.Model.FavoriteMovies;

public class FavoriteMoviesModel : IFavoriteMoviesModel, IAlertNotifier
{

    private IFavoriteMoviesClient _client;
    private IAlertAggregator _alertAggregator;

    public FavoriteMoviesModel(IFavoriteMoviesClient client, IAlertAggregator alertAggregator)
    {
        _client = client;
        _alertAggregator = alertAggregator;
    }

    public async Task<IList<Movie>> GetFavoriteMovies(string userToken = null, string UID = null)
    {
        return await _client.GetFavoriteMovies(userToken, UID);
    }

    public async Task AddToFavoriteMovies(string bearerToken, string movieId)
    {
        try
        {
            await _client.AddToFavoriteMovies(bearerToken, movieId);
            FireAlertEvent(AlertBoxHelper.AlertType.AddFavoriteMovieSuccess,
                $"Successfully added movie to your favourites!");
        }
        catch (Exception e)
        {
            FireAlertEvent(AlertBoxHelper.AlertType.AddFavoriteMovieFail,
                $"Could not add move to your favourites. Reason {e.Message}");
            throw;
        }
        
    }

    public async Task RemoveFromFavoriteMovies(string bearerToken, string movieId)
    {
        try
        {
            await _client.AddToFavoriteMovies(bearerToken, movieId);
            FireAlertEvent(AlertBoxHelper.AlertType.RemoveFavouriteMovieSuccess,
                $"Successfully removed movie from favourites!");
        }
        catch (Exception e)
        {
            FireAlertEvent(AlertBoxHelper.AlertType.RemoveFavouriteMovieFail,
                $"Could not remove movie from favourites. Reason {e.Message}");
            throw;
        }
    }

    public void FireAlertEvent(AlertBoxHelper.AlertType type, string message)
    {
        _alertAggregator.BroadCast(new AlertEventArgs
        {
            Type = type,
            Message = message
        });
    }
}