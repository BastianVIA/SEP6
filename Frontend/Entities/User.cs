using Frontend.Service;

namespace Frontend.Entities;

public class User
{

    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    
    public List<UserRating> UserRatings { get; set; }
    
    public byte[]? ProfilePicture { get; set;}
    public List<Movie> FavoriteMovies { get; set; }
    
    public int RatedMovies { get; set; }

    private double _averageOfUserRatings;
    public double AverageOfUserRatings
    {
        get { return Math.Round(_averageOfUserRatings, 1); }
        set { _averageOfUserRatings = value; }
    }
    
    public ICollection<Int32Int32ValueTuple> RatingDataPoints { get; set; }
    
    
    public DateTime? LastLogin { get; set; }
    
    
}