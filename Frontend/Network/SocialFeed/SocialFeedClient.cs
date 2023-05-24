using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.SocialFeed;

public class SocialFeedClient : NSwagBaseClient, ISocialFeedClient
{
    public async Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var response = await _api.UserFeedAsync();
        List<UserFeed> userFeeds = new List<UserFeed>();
        foreach (var userFeed in response.GetPostsForUsersResponse.FeedPostDtos)
        {
            ActivityData activityData = new ActivityData();
            if (userFeed.ActivityDataDto != null)
            {
                activityData = new ActivityData
                {
                    MovieId = userFeed.ActivityDataDto.MovieId,
                    MovieTitle = userFeed.ActivityDataDto.MovieTitle,
                    NewRating = userFeed.ActivityDataDto.NewRating,
                    OldRating = userFeed.ActivityDataDto.OldRating,
                    ReviewBody = userFeed.ActivityDataDto.ReviewBody
                };
            }

            List<Comment> comments = new List<Comment>();
            foreach (var comment in userFeed.Comments)
            {
                comments.Add(new Comment
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    DisplayNameOfUser = comment.DisplayNameOfUser,
                    Content = comment.Content,
                    TimeStamp = comment.TimeStamp
                });
            }

            userFeeds.Add(new UserFeed
            {
                Id = userFeed.Id,
                UserId = userFeed.UserId,
                Topic = userFeed.Topic,
                TimeOfActivity = userFeed.TimeOfActivity,
                ActivityData = activityData,
                Comments = comments,
                DisplayName = userFeed.UserDisplayname,
                NumberOfReactions = userFeed.NumberOfReactions
            });
        }
        return userFeeds;
    }

    public async Task ReactToSocialFeed(string userToken, string postId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var body = new ReactToPostRequest
        {
            PostId = postId,
            Reation = ReactToPostRequestReation.LIKE
        };
        await _api.ReactToPostAsync(body);
    }

    public async Task CommentOnPost(string userToken, string postId, string comment)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var body = new CommentOnPostRequest
        {
            PostId = postId,
            Comment = comment
        };
        await _api.CommentOnPostAsync(body);
    }

    public async Task<List<UserFeed>> GetOwnSocialFeed(string userId, int? pageNumber = null)
    {
        var response = await _api.PostAsync(userId, pageNumber);
        List<UserFeed> userFeeds = new List<UserFeed>();
        foreach (var userFeed in response.GetPostsForUsersResponse.FeedPostDtos)
        {
            ActivityData activityData = new ActivityData();
            if (userFeed.ActivityDataDto != null)
            {
                activityData = new ActivityData
                {
                    MovieId = userFeed.ActivityDataDto.MovieId,
                    MovieTitle = userFeed.ActivityDataDto.MovieTitle,
                    NewRating = userFeed.ActivityDataDto.NewRating,
                    OldRating = userFeed.ActivityDataDto.OldRating,
                    ReviewBody = userFeed.ActivityDataDto.ReviewBody
                };
            }

            List<Comment> comments = new List<Comment>();
            foreach (var comment in userFeed.Comments)
            {
                comments.Add(new Comment
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    DisplayNameOfUser = comment.DisplayNameOfUser,
                    Content = comment.Content,
                    TimeStamp = comment.TimeStamp
                });
            }

            userFeeds.Add(new UserFeed
            {
                Id = userFeed.Id,
                UserId = userFeed.UserId,
                Topic = userFeed.Topic,
                TimeOfActivity = userFeed.TimeOfActivity,
                ActivityData = activityData,
                Comments = comments,
                DisplayName = userFeed.UserDisplayname,
                NumberOfReactions = userFeed.NumberOfReactions
            });
        }
        return userFeeds;
    }

    public SocialFeedClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}