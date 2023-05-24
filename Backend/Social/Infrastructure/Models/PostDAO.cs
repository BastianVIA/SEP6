using System.ComponentModel.DataAnnotations;
using Backend.SocialFeed.Domain;

namespace Backend.SocialFeed.Infrastructure;

public class PostDAO
{
    [Key]
    public string Id;
    public string UserId  { get; set; }
    public Activity Topic { get; set; }
    public ActivityDAO? ActivityData { get; set; }
    public DateTime TimeOfActivity { get; set; }
    public List<CommentDAO>? Comments { get; set; }
    public List<ReactionEntryDAO>? Reactions { get; set; }
}