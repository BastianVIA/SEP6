using Frontend.Network;
using Frontend.Network.UserProfiles;

namespace Frontend.Model.UserProfiles;

public class UserProfilesModel: NSwagBaseClient, IUserProfilesModel
{
    private IUserProfileClient _client;
    public double AverageRating { get; set; }

    public UserProfilesModel(IUserProfileClient client,IConfiguration configuration,IHttpClientFactory clientFactory):base(clientFactory,configuration)
    {
        _client = client;
    }

    public async Task<Entities.User> GetUserProfile(string userId)
    {
        var user = await _client.GetUserProfile(userId);
        CalculateAverage(user);
        return user;
    }

    private void CalculateAverage(Entities.User user)
    {
        if (user.UserRatings.Count == 0)
        {
            AverageRating = 0;
            Console.WriteLine("userRating count" +user.UserRatings.Count);

            return;
        }

        double counter = 0;

        foreach (var ratings in user.UserRatings)
        {
            counter += ratings.NumberOfStars;
        }

        AverageRating = counter / user.UserRatings.Count;
    }

}


