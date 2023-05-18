using Backend.Foundation;

namespace Backend.Movie.Domain;

public class Movie : BaseDomain
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public Rating? Rating { get; set; }
    public List<Person>? Actors { get; set; }
    public List<Person>? Directors { get; set; }

    public void NewRating(int newRating)
    {
        if (Rating == null)
        {
            Rating = new Rating();
        }
        Rating.NewRating(newRating);
    }

    public void UpdateRating(int oldRating, int newRating)
    {
        if (Rating == null)
        {
            throw new KeyNotFoundException($"Tried to update rating {Id}, but it does not have any ratings");
        }
        Rating.UpdateRating(oldRating, newRating);
    }

    public void RemoveRating(int ratingToRemove)
    {
        if (Rating == null)
        {
            throw new KeyNotFoundException($"Tried to remove rating from {Id}, but it does not have any ratings");
        }
        Rating.RemoveRating(ratingToRemove);
    }
}