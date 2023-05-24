using Frontend.Entities;
using Frontend.Events;
using Frontend.Network;
using Frontend.Network.MovieDetail;
using Frontend.Service;

namespace Frontend.Model.MovieDetail;

public class MovieDetailModel :IMovieDetailModel, IAlertNotifier

{
    private IMovieDetailClient _client;
    private IAlertAggregator _alertAggregator;
    
    public MovieDetailModel(IMovieDetailClient client,IAlertAggregator alertAggregator)
    {
        _client = client;
        _alertAggregator = alertAggregator;
    }

    public async Task<Movie?> GetMovieDetails(string movieId, string userToken)
    {
        return await _client.GetMovieDetails(movieId, userToken);
    }

    public async Task SetMovieRating(string? userToken, string movieId, int? rating = null)
    {
        try
        {
            await _client.SetMovieRating(userToken, movieId, rating);
            if(rating == null) return;
            FireAlertEvent(AlertBoxHelper.AlertType.SetRatingSuccess,
                $"New rating registered.");
        }
        catch (Exception e)
        {
            FireAlertEvent(AlertBoxHelper.AlertType.SetRatingFail,
                $"Could not set new rating. Reason: {e.Message}");
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