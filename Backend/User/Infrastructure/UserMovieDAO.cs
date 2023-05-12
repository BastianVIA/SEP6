using System.ComponentModel.DataAnnotations;

namespace Backend.User.Infrastructure;

public class UserMovieDAO
{
    [Key]
    public string Id { get; set; }
}