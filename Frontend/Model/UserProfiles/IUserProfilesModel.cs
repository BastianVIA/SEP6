namespace Frontend.Model.UserProfiles;

public interface IUserProfilesModel
{
    Task<Entities.User> GetUserProfile(string userId);
    public double AverageRating { get; set; }

}