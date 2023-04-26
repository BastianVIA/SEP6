namespace Frontend.Entities;

[Serializable]
public class Movie
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }

    public Uri ImagePath { get; set; }
}