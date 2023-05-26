using Frontend.Components;

namespace Frontend.Model.UserProfiles;

public interface IUserProfilesModel
{
    Task<Entities.User> GetUserProfile(string userId);
    public double AverageRating { get; set; }
    public Task FollowUser(string userToken, string userId);
    public Task UnFollowUser(string userToken, string userId);
    public Task<bool> IsFollowingUser(string userToken, string ownUserId, string profileUserId);
    public Task<List<Entities.User>> GetFollowingUsers(string userToken, string ownUserId);
}