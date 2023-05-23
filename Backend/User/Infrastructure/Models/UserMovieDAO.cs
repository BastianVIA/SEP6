using System.ComponentModel.DataAnnotations;

namespace Backend.User.Infrastructure;

public class UserMovieDAO
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public UserDAO User { get; set; }

}