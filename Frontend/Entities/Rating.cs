namespace Frontend.Entities;

[Serializable]
public class Rating
{
    public double? AverageRating { get; set; }
    public int? RatingCount { get; set; }
}