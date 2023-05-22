using Frontend.Network.UserProfilePicture;

namespace Frontend.Model.UserProfilePicture;

public class UserProfilePictureModel : IUserProfilePictureModel
{
    private IUserProfilePictureClient _client;

    public UserProfilePictureModel()
    {
        _client = new UserProfilePictureClient();
    }
    
    public async Task UploadProfilePicture(string userToken, byte[] profilePicture)
    {
        await _client.UploadProfilePicture(userToken, profilePicture);
    }

    public async Task<byte[]> GetProfilePicture(string userId)
    {
        return await _client.GetProfilePicture(userId);
    }
}