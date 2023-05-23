using System.ComponentModel.DataAnnotations;
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
public enum TypeOfReaction
{
    LIKE
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
        Comments.Add(new Comment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Contents = Content,
            TimeStamp = DateTime.Now
        });
    }

    public void PutReaction(string userId, TypeOfReaction typeOfReaction)
    {
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
