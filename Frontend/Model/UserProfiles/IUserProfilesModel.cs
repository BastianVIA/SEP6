namespace Frontend.Model.UserProfiles;

public interface IUserProfilesModel
{
    Task<Entities.User> GetUserProfile(string userId, string? userToken);
}