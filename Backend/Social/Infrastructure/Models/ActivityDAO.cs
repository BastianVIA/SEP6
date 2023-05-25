using System.ComponentModel.DataAnnotations;

namespace Backend.Social.Infrastructure.Models;

public class ActivityDAO
{
    [Key]
    public string Id { get; set; }
    public string? MovieId { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
    
    public string? ReviewBody { get; set; }
    public string PostId { get; set; }
    public PostDAO PostThisBelongsTo { get; set; }
}