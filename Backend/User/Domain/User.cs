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
    public List<string>? FavoriteMovies { get; set; }
    public List<UserRating>? Ratings { get; set; }
    
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
        var newRating = new UserRating(movieId, rating);
        Ratings.Add(newRating);
        AddDomainEvent(new CreatedRatingEvent(Id, newRating));
    }
    
    public void RemoveRating(string movieId)
    {
        for (int i = Ratings.Count - 1; i >= 0; i--)
        {
            var rating = Ratings[i];
            if (movieId == rating.MovieId)
            {
                Ratings.RemoveAt(i);
                AddDomainEvent(new RemovedRatingEvent(Id, rating));
                return;
            }
        }

        throw new KeyNotFoundException(
            $"Tried to remove rating from movie with id: {movieId}, from user with id: {Id}, but could not find any");
    }

    public void UpdateRating(string requestMovieId, int rating)
    {
        foreach (var userRating in Ratings)
        {
            if (userRating.MovieId == requestMovieId)
            {
                var prevRating = userRating.NumberOfStars;
                userRating.NumberOfStars = rating;
                AddDomainEvent(new UpdatedRatingEvent(Id, requestMovieId, prevRating, rating));
                return;
            }
        }
        
        throw new KeyNotFoundException(
            $"Tried to update rating from movie with id: {requestMovieId}, from user with id: {Id}, but could not find the rating");
    }

    public int GetRatingForMovie(string movieId)
    {
        foreach (var rating in Ratings)
        {
            if (rating.MovieId == movieId)
            {
                return rating.NumberOfStars;
            }
        }
        throw new KeyNotFoundException(
            $"Tried to get rating from movie with id: {movieId}, from user with id: {Id}, but could not find the rating");
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