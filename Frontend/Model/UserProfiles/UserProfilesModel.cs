using Frontend.Network.UserProfiles;

namespace Frontend.Model.UserProfiles;

public class UserProfilesModel: IUserProfilesModel
{
    private IUserProfileClient _client;

    public UserProfilesModel(UserProfileClient client)
    {
        _client = client;
    }
    
    
    public async Task<Entities.User> GetUserProfile(string userId, string? userToken)
    {
        return await _client.GetUserProfile(userId,userToken);

    }
    
}