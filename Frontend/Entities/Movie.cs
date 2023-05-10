namespace Frontend.Entities;

[Serializable]
public class Movie
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int? ReleaseYear { get; set; }
    public Uri? PosterUrl { get; set; }
    public List<Actor>? Actors { get; set; }
    public List<Director>? Directors { get; set; }
    public Rating? Rating { get; set; }
    public string? Resume { get; set; }
}