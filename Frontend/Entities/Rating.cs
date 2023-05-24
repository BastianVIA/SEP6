namespace Frontend.Entities;

[Serializable]
public class Rating
{
    private double? _averageRating;

    public double? AverageRating
    {
        get
        {
            if (_averageRating.HasValue)
                return Math.Round(_averageRating.Value, 1);
            else
            {
                return null;
            }
        }
        set { _averageRating = value; }

    }
    public int? RatingCount { get; set; }
}