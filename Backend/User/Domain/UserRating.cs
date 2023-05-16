namespace Backend.User.Domain;

public class UserRating
{
    public string MovieId { get; set; }
    public int NumberOfStars { get; set; }

    public UserRating(string movieId, int numberOfStars)
    {
        MovieId = movieId;
        NumberOfStars = numberOfStars;
    }
}