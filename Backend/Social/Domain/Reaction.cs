namespace Backend.SocialFeed.Domain;

public enum TypeOfReaction
{
    LIKE
}

public class Reaction
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public TypeOfReaction TypeOfReaction { get; set; }
}