using System.ComponentModel.DataAnnotations;

namespace Backend.Social.Infrastructure.Models;

public class CommentDAO
{
    [Key]
    public string Id  { get; set; }
    public string UserId { get; set; }
    public string Contents { get; set; }
    public DateTime TimeStamp { get; set; }
}