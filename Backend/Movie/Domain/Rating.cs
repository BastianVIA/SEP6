using System.ComponentModel.DataAnnotations;

namespace Backend.Movie.Domain;

public class Rating
{
    public float AverageRating  { get; set; }
    public int Votes { get; set; }
    private const int MaxRating = 10;
    private const int MinRating = 1;

    public void NewRating(int newRating)
    {
        if (newRating > MaxRating || newRating < MinRating)
        {
            throw new ValidationException(
                $"Trying to add a rating of: {newRating}, this is not in the specified range from {MinRating} to {MaxRating}");
        }
        var totalRating = AverageRating * Votes;
        Votes++;
        AverageRating = (totalRating + newRating) / Votes;
    }
    
    public void UpdateRating(int oldRating, int newRating)
    {
        if (newRating > MaxRating || newRating < MinRating)
        {
            throw new ValidationException(
                $"Trying to update rating to: {newRating}, this is not in the specified range from {MinRating} to {MaxRating}");
        }
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