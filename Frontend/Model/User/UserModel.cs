using System.Net.Http.Headers;
using Frontend.Events;
using Frontend.Network;
using Frontend.Network.User;
using Frontend.Service;

namespace Frontend.Model.User;

public class UserModel : NSwagBaseClient, IUserModel, IAlertNotifier
{
    private IUserClient _client;
    private IAlertAggregator _alertAggregator;

    public UserModel(IAlertAggregator alertAggregator)
    {
        _client = new UserClient();
        _alertAggregator = alertAggregator;
    }

    public async Task CreateUser(string userToken,string displayName, string email)
    {
        await _client.CreateUser(userToken, displayName, email);
    }

    public async Task SetReview(string userToken, string movieId, string review)
    {
        try
        {
            await _client.SetReview(userToken, movieId, review);
            FireAlertEvent(AlertBoxHelper.AlertType.CreateReviewSuccess,
                "You have successfully created a new review!");
        }
        catch (Exception e)
        {
            FireAlertEvent(AlertBoxHelper.AlertType.CreateReviewFail,
                $"Could not create new review. Reason: {e.Message}");
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