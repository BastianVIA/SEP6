namespace Backend.SocialFeed.Domain;

public class ActivityData
{
    public string? MovieId { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
    public string? ReviewBody { get; set; }
}