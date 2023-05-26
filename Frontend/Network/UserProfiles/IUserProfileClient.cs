using Frontend.Service;

namespace Frontend.Network.UserProfiles;

public interface IUserProfileClient
{
    public Task<Entities.User> GetUserProfile(string userId);

    public Task FollowUser(string userToken, string userId);
    public Task UnFollowUser(string userToken, string userId);
    public Task<GetFollowingResponse> GetFollowingUsers(string userToken, string ownUserId);
}