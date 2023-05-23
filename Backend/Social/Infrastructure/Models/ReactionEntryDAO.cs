using Backend.SocialFeed.Domain;

namespace Backend.SocialFeed.Infrastructure;

public class ReactionEntryDAO
{
    public string UserId { get; set; }
    public string PostId { get; set; }
    public TypeOfReaction TypeOfReaction { get; set; }
    public PostDAO Post { get; set; }
}