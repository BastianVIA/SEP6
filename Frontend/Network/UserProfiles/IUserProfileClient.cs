using Frontend.Service;

namespace Frontend.Network.UserProfiles;

public interface IUserProfileClient
{
    public Task<Entities.User> GetUserProfile(string userId);

    Task UpdateUserProfile(Entities.User user);
    public Task FollowUser(string userToken, string userId);
    public Task<GetFollowingResponse> GetFollowingUsers(string userToken, string ownUserId);
}