using Backend.Foundation;

namespace Backend.SocialFeed.Domain;

public enum Activity
{
    FavoriteMovie,
    CreatedRating,
    UpdatedRating,
    RemovedRating,
    NewUser,
    UnFavoriteMovie,
    CreatedReview
    
}

public class Post : BaseDomain
{
    public Guid Id;
    public string UserId  { get; set; }
    public Activity Topic { get; set; }
    public ActivityData? ActivityData { get; set; }
    public DateTime TimeOfActivity { get; set; }

    public Post() { }
    
    public Post(string userId, Activity topic, ActivityData? activityData = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Topic = topic;
        ActivityData = activityData;
        TimeOfActivity = DateTime.Now;
    }
}