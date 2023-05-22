using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.SocialFeed;

public class SocialFeedClient : NSwagBaseClient, ISocialFeedClient
{
    public async Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        GetFeedForUserResponse response;
        try
        {
            response = await _api.UserFeedAsync(pageNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        List<UserFeed> userFeeds = new List<UserFeed>();
        foreach (var userFeed in response.FeedPostDtos)
        {
            ActivityData activityData = new ActivityData();
            if (userFeed.ActivityDataDto != null)
            {
                activityData = new ActivityData
                {
                    MovieId = userFeed.ActivityDataDto.MovieId,
                    NewRating = userFeed.ActivityDataDto.NewRating,
                    OldRating = userFeed.ActivityDataDto.OldRating
                };
            }
            
            userFeeds.Add(new UserFeed
            {
                Id = userFeed.Id,
                UserId = userFeed.UserId,
                Topic = userFeed.Topic,
                TimeOfActivity = userFeed.TimeOfActivity,
                ActivityData = activityData
            });
        }

        return userFeeds;
    }
}