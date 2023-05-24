using Frontend.Network.UserProfiles;

namespace Frontend.Model.UserProfiles;

public class UserProfilesModel: IUserProfilesModel
{
    private IUserProfileClient _client;
    public double AverageRating { get; set; }
   
    public UserProfilesModel()
    {
        _client = new UserProfileClient();
    }

    public async Task UpdateUserProfile(Entities.User user)
    {
        await _client.UpdateUserProfile(user);
    }

    public async Task FollowUser(string userToken, string userId)
    {
        await _client.FollowUser(userToken, userId);
    }

    public async Task<bool> IsFollowingUser(string userToken, string ownUserId, string profileUserId)
    {
        var followedUserIds = await _client.IsFollowingUser(userToken, ownUserId);
        foreach (var followedUserId in followedUserIds)
        {
            if (profileUserId.Equals(followedUserId))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<List<string>> GetFollowingUserIds(string userToken, string ownUserId)
    {
        return await _client.GetFollowingUsers(userToken, ownUserId);
    }

    public async Task<Entities.User> GetUserProfile(string userId)
    {
        var user = await _client.GetUserProfile(userId);
        return user;
    }
    
}


