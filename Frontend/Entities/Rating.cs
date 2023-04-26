namespace Frontend.Entities;

[Serializable]
public class Rating
{
    public float? AverageRating { get; set; }
    public int? RatingCount { get; set; }
}