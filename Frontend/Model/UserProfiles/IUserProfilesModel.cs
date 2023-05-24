using Frontend.Components;

namespace Frontend.Model.UserProfiles;

public interface IUserProfilesModel
{
    Task<Entities.User> GetUserProfile(string userId);
    public double AverageRating { get; set; }

    Task UpdateUserProfile(Entities.User user);
    
    public Task FollowUser(string userToken, string userId);
    public Task<bool> IsFollowingUser(string userToken, string ownUserId, string profileUserId);
    public Task<List<string>> GetFollowingUserIds(string userToken, string ownUserId);
}