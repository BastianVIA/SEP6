using System.ComponentModel.DataAnnotations;

namespace Backend.User.Domain;

public class UserRating
{
    public string MovieId { get; set; }
    public int NumberOfStars { get; set; }
    
    private const int MaxRating = 10;
    private const int MinRating = 1;
    public UserRating(){}
    public UserRating(string movieId, int numberOfStars)
    {
        if (movieId == "")
        {
            throw new ValidationException(
                $"Trying to add a UserRating with empty movieId, this is not allowed, a UserRating has to have a movieId");
        } 
        if (numberOfStars > MaxRating || numberOfStars < MinRating)
        {
            throw new ValidationException(
                $"Trying to add a rating of: {numberOfStars}, this is not in the specified range from {MinRating} to {MaxRating}");
        }        
        
        MovieId = movieId;
        NumberOfStars = numberOfStars;
    }
}