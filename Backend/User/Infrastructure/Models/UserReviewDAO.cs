namespace Backend.User.Infrastructure;

public class UserReviewDAO
{
    public string UserId { get; set; }
    
    public string MovieId { get; set; }
    
    public string Body { get; set; }

    public UserDAO User { get; set; }
}