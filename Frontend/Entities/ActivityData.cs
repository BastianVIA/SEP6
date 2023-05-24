namespace Frontend.Entities;

public class ActivityData
{
    public string MovieId { get; set; }
    public string MovieTitle { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
    public string? ReviewBody { get; set; }
}