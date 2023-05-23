namespace Frontend.Network.UserProfiles;

public interface IUserProfileClient
{
    public Task<Entities.User> GetUserProfile(string userId);

    Task UpdateUserProfile(Entities.User user);
}