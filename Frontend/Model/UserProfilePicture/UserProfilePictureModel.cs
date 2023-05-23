using Frontend.Events;
using Frontend.Network.UserProfilePicture;

namespace Frontend.Model.UserProfilePicture;

public class UserProfilePictureModel : IUserProfilePictureModel, IAlertNotifier
{
    private IUserProfilePictureClient _client;
    private IAlertAggregator _alertAggregator;

    public UserProfilePictureModel(IAlertAggregator alertAggregator)
    {
        _client = new UserProfilePictureClient();
        _alertAggregator = alertAggregator;
    }
    
    public async Task UploadProfilePicture(string userToken, byte[] profilePicture)
    {
        try
        {
            await _client.UploadProfilePicture(userToken, profilePicture);
            FireAlertEvent(AlertBoxHelper.AlertType.UploadProfileImageSuccess,
                "Successfully uploaded new profile picture!");
        }
        catch (Exception e)
        {
            FireAlertEvent(AlertBoxHelper.AlertType.UploadProfileImageFail,
                $"Could not upload profile picture. Reason: {e.Message}");
            throw;
        }
        
    }

    public async Task<byte[]> GetProfilePicture(string userId)
    {
        return await _client.GetProfilePicture(userId);
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