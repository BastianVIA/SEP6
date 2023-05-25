using System.ComponentModel.DataAnnotations;
using Backend.Social.Domain;

namespace Backend.Social.Infrastructure.Models;

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