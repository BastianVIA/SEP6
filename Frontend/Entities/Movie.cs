﻿namespace Frontend.Entities;

[Serializable]
public class Movie
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }
    public bool? IsFavorite { get; set;}
    public int? UserRating { get; set; }
    public Uri? PosterUrl { get; set; }
    
    public string? MovieTrailer { get; set; }
    public List<Person>? Actors { get; set; }
    public List<Director>? Directors { get; set; }
    public Rating? Rating { get; set; }
    public string? Resume { get; set; }
}