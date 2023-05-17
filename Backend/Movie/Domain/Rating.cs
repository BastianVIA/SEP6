namespace Backend.Movie.Domain;

public class Rating
{
    public float AverageRating  { get; set; }
    public int Votes { get; set; }

    public void NewRating(int newRating)
    {
        var totalRating = AverageRating * Votes;
        Votes++;
        AverageRating = (totalRating + newRating) / Votes;
    }
    
    public void UpdateRating(int oldRating, int newRating)
    {
        var totalRating = AverageRating * Votes;
        AverageRating = (totalRating - oldRating + newRating) / Votes;
    }

    public void RemoveRating(int ratingToRemove)
    {
        var totalRating = AverageRating * Votes;
        Votes--;
        if (Votes == 0)
        {
            AverageRating = 0;
        }
        else
        {
            AverageRating = (totalRating - ratingToRemove) / Votes;
        }
    }
}