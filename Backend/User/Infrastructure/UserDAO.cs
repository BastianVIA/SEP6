using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Backend.User.Infrastructure;

public class UserDAO
{
    [Key]
    public string Id { get; set; }
    
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public List<UserMovieDAO>? FavoriteMovies { get; set; }
    public List<UserRatingDAO>? UserRatings { get; set; }

}