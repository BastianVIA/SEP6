using Backend.Social.Domain;

namespace Backend.Social.Infrastructure.Models;

public class ReactionEntryDAO
{
    public string UserId { get; set; }
    public string PostId { get; set; }
    public TypeOfReaction TypeOfReaction { get; set; }
    public PostDAO Post { get; set; }
}