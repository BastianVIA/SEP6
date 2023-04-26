namespace Backend.Movie.Domain;

public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public Rating? Rating { get; set; }
    public List<Person>? Actors { get; set; }
    public List<Person>? Directors { get; set; }
}