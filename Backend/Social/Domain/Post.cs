using System.ComponentModel.DataAnnotations;
using Backend.Foundation;

namespace Backend.Social.Domain;

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

public enum TypeOfReaction
{
    LIKE,
    LOVE,
    HAHA,
    WOW
}

public class Post : BaseDomain
{
    public Guid Id;
    public string UserId { get; set; }
    public Activity Topic { get; set; }
    public ActivityData? ActivityData { get; set; }
    public DateTime TimeOfActivity { get; set; }
    public List<Comment>? Comments { get; set; }
    public Dictionary<string, TypeOfReaction>? Reactions { get; set; }

    public Post()
    {
    }

    public Post(string userId, Activity topic, ActivityData? activityData = null)
    {
        if (userId == "")
        {
            throw new ValidationException(
                $"Trying to add a post with empty userId, this is not allowed, a post has to have a user");
        }
        if (!System.Enum.IsDefined(typeof(Activity), topic))
        {
            throw new ValidationException(
                $"Invalid topic value {topic} provided. Only values from the Activity enum are allowed to be used as topic.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Topic = topic;
        ActivityData = activityData;
        TimeOfActivity = DateTime.Now;
        AddDomainEvent(new PostCreatedEvent(Id));
    }

    public void AddComment(string userId, string Content)
    {
        if (Comments == null)
        {
            Comments = new List<Comment>();
        }

        Comments.Add(new Comment(userId, Content));
    }

    public void PutReaction(string userId, TypeOfReaction typeOfReaction)
    {
        if (userId == "")
        {
            throw new ValidationException(
                $"Trying to add a Reaction to a post with empty userId, this is not allowed, a Reaction has to have a user");
        }
        if (Reactions == null)
        {
            Reactions = new Dictionary<string, TypeOfReaction>();
        }

        if (Reactions.ContainsKey(userId))
        {
            updateExistingReaction(userId, typeOfReaction);
        }
        else
        {
            Reactions.Add(userId, typeOfReaction);
        }
    }

    public TypeOfReaction? GetUsersReaction(string userId)
    {
        if (Reactions == null)
        {
            return null;
        }

        if (Reactions.TryGetValue(userId, out TypeOfReaction reaction))
        {
            return reaction;
        }

        return null;
    }

    private void updateExistingReaction(string userId, TypeOfReaction typeOfReaction)
    {
        if (Reactions == null)
        {
            throw new ValidationException("Tried to update users reaction, but could not find any reactions");
        }

        if (Reactions[userId] != typeOfReaction)
        {
            Reactions[userId] = typeOfReaction;
        }
        else
        {
            removeReaction(userId);
        }
    }

    private void removeReaction(string userId)
    {
        Reactions.Remove(userId);
    }
}