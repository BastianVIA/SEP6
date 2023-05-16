namespace Backend.User.Infrastructure;

public class UserRatingDAO
{
    public string UserId { get; set; }
    public string MovieId { get; set; }
    public int NumberOfStars { get; set; }
    public UserDAO User { get; set; }

}