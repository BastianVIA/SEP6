using System.ComponentModel.DataAnnotations;
using Backend.SocialFeed.Domain;

namespace Backend.SocialFeed.Infrastructure;

public class ReactionDAO
{
    [Key]
    public string Id  { get; set; }
    public string UserId { get; set; }
    public TypeOfReaction TypeOfReaction { get; set; }
}