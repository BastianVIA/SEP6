namespace Backend.User.Domain;

public class UserReview
{
    public string MovieId { get; set; }
    public string ReviewBody { get; set; }

    public UserReview(){}
    public UserReview(string movieId, string reviewBody)
    {
        MovieId = movieId;
        ReviewBody = reviewBody;
    }
}