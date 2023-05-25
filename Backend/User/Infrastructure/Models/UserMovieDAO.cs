namespace Backend.User.Infrastructure.Models;

public class UserMovieDAO
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime TimeMovieWasAdded { get; set; }
    public UserDAO User { get; set; }

}