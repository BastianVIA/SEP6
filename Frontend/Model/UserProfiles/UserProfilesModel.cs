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

    public async Task UnFollowUser(string userToken, string userId)
    {
        await _client.UnFollowUser(userToken, userId);
    }

    public async Task<bool> IsFollowingUser(string userToken, string ownUserId, string profileUserId)
    {
        var followingResponse = await _client.GetFollowingUsers(userToken, ownUserId);
        foreach (var followedUser in followingResponse.FollowingUserDtos)
        {
            if (profileUserId.Equals(followedUser.Id))
            {
                return true;
            }
        }
        return false;
    }

    public async Task<List<Entities.User>> GetFollowingUsers(string userToken, string ownUserId)
    {
        var response = await _client.GetFollowingUsers(userToken, ownUserId);
        List<Entities.User> followedUsers = new List<Entities.User>();
        foreach (var followingUser in response.FollowingUserDtos)
        {
            followedUsers.Add(new Entities.User
            {
                Id = followingUser.Id,
                Username = followingUser.DisplayName
            });
        }

        return followedUsers;
    }

    public async Task<Entities.User> GetUserProfile(string userId)
    {
        var user = await _client.GetUserProfile(userId);
        return user;
    }
    
    
}


