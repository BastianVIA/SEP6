using System.ComponentModel.DataAnnotations;

namespace Backend.SocialFeed.Infrastructure;

public class CommentDAO
{
    [Key]
    public string Id  { get; set; }
    public string UserId { get; set; }
    public string Contents { get; set; }
}