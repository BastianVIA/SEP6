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
    CreatedReview,
}

public class Post : BaseDomain
{
    public Guid Id;
    public string UserId  { get; set; }
    public Activity Topic { get; set; }
    public ActivityData? ActivityData { get; set; }
    public DateTime TimeOfActivity { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Reaction>? Reactions { get; set; }

    public Post() { }
    
    public Post(string userId, Activity topic, ActivityData? activityData = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Topic = topic;
        ActivityData = activityData;
        TimeOfActivity = DateTime.Now;
    }

    public void AddComment(string userId, string Content)
    {
        if (Comments == null)
        {
            Comments = new List<Comment>();
        }
        Comments.Add(new Comment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Contents = Content
        });
    }

    public void AddReaction(string userId, TypeOfReaction typeOfReaction)
    {
        if (Reactions == null)
        {
            Reactions = new List<Reaction>();
        }
        Reactions.Add(new Reaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TypeOfReaction = typeOfReaction
        });
    }
}