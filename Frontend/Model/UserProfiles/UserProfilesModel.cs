using Frontend.Network.UserProfiles;

namespace Frontend.Model.UserProfiles;

public class UserProfilesModel: IUserProfilesModel
{
    private IUserProfileClient _client;
    public double AverageRating { get; set; }
   
    public UserProfilesModel(IUserProfileClient client)
    {
        _client = client;
    }

    public async Task UpdateUserProfile(Entities.User user)
    {
        await _client.UpdateUserProfile(user);
    }
    
    public async Task<Entities.User> GetUserProfile(string userId)
    {
        var user = await _client.GetUserProfile(userId);
        return user;
    }



}


