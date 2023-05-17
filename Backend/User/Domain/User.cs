using System.Data.Common;
using Backend.Movie.Domain;
using NLog;

namespace Backend.User.Domain;

public class User : Foundation.BaseDomain
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public List<string> FavoriteMovies { get; set; }
    public List<UserRating> Ratings { get; set; }
    
    public double AverageOfUserRatings { get; set; }

    public User()
    {
        FavoriteMovies = new List<string>();
        Ratings = new List<UserRating>();
    }
    public bool HasAlreadyFavoritedMovie(string movieId)
    {
        foreach (var favoriteMovie in FavoriteMovies)
        {
            if (favoriteMovie == movieId)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasAlreadyRatedMovie(string movieId)
    {
        foreach (var r in Ratings)
        {
            if (r.MovieId == movieId)
            {
                return true;
            }
        }

        return false;
    }

    public void AddRating(string movieId, int rating)
    {
        Ratings.Add(new UserRating(movieId, rating));
        AddDomainEvent(new UpdatedRatingEvent
        {
            UserId = Id,
            MovieId = movieId,
            NewRating = rating
        });
    }
    
    public void RemoveRating(string movieId)
    {
        foreach (var rating in Ratings)
        {
            if (movieId == rating.MovieId)
            {
                Ratings.Remove(rating);
                AddDomainEvent(new UpdatedRatingEvent
                {
                    UserId = Id,
                    MovieId = movieId
                });
                return;
            }
        }
    }
    
    public void SetRatingAvg()
    {
        double count = 0;
        foreach (var rating in Ratings)
        {
            count += rating.NumberOfStars;
        }

        AverageOfUserRatings = count / Ratings.Count;
    }
}