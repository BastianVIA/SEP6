using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.User.Infrastructure;

public class UserDAO
{
    [Key]
    public string Id { get; set; }
    public List<UserMovieDAO>? FavoriteMovies { get; set; }
    
}