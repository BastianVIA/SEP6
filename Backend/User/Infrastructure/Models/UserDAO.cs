using System.ComponentModel.DataAnnotations;

namespace Backend.User.Infrastructure.Models;

public class UserDAO
{
    [Key]
    public string Id { get; set; }
    
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public List<UserMovieDAO>? FavoriteMovies { get; set; }
    public List<UserRatingDAO>? UserRatings { get; set; }
    public List<UserReviewDAO>? UserReviews { get; set; }

}