namespace Frontend.Entities;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    
    public Rating Rating { get; set; }
    
    public string ProfilePicture { get; set;}
    public List<Movie> FavoriteMovies { get; set; }
    
    
}