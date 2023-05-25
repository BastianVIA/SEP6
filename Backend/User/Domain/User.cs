using System.ComponentModel.DataAnnotations;
using Backend.Foundation;

namespace Backend.User.Domain;

public class User : BaseDomain
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public List<UserFavoriteMovie>? FavoriteMovies { get; set; }
    public List<UserRating>? Ratings { get; set; }
    
    public List<UserReview>? Reviews { get; set; }
    
    public float AverageOfUserRatings { get; set; }

    public User()
    {
    }
    public User(string id, string displayName, string email)
    {
        Id = id;
        DisplayName = displayName;
        Email = email;
        AddDomainEvent(new UserCreated(Id));
    }

    public bool HasAlreadyFavoritedMovie(string movieId)
    {
        if (FavoriteMovies == null)
        {
            return false;
        }

        foreach (var favoriteMovie in FavoriteMovies)
        {
            if (favoriteMovie.MovieId == movieId)
            {
                return true;
            }
        }

        return false;
    }

    public void AddRating(string movieId, int rating)
    {
        if (Ratings == null)
        {
            Ratings = new List<UserRating>();
        }

        var newRating = new UserRating(movieId, rating);
        Ratings.Add(newRating);
        AddDomainEvent(new CreatedRatingEvent(Id, newRating));
    }

    public void AddReview(string movieId, string reviewBody)
    {
        if (Reviews == null)
        {
            Reviews = new List<UserReview>();
        }
    
        var newReview = new UserReview(movieId, reviewBody);
        Reviews.Add(newReview);
        AddDomainEvent(new CreateReviewEvent(Id, newReview));
    }

    public void RemoveFavorite(string movieId)
    {
        if (FavoriteMovies == null)
        {
            throw new ValidationException("Tried removing from Favortie movies, but no favorite Movies exists");
        }

        for (int i = FavoriteMovies.Count - 1; i >= 0; i--)
        {
            var movie = FavoriteMovies[i];

            if (movieId == movie.MovieId)
            {
                FavoriteMovies.RemoveAt(i);
                AddDomainEvent(new UnFavoritedMovie(Id, movieId));
                return;
            }
        }
    }

    public bool HasAlreadyRatedMovie(string movieId)
    {
        if (Ratings == null)
        {
            return false;
        }

        foreach (var r in Ratings)
        {
            if (r.MovieId == movieId)
            {
                return true;
            }
        }

        return false;
    }
    
    public void AddFavoriteMovie(string movieId)
    {
        if (FavoriteMovies == null)
        {
            FavoriteMovies = new List<UserFavoriteMovie>();
        }

        FavoriteMovies.Add(new UserFavoriteMovie(movieId));
        AddDomainEvent(new FavoritedMovie(Id, movieId));
    }

    public void RemoveRating(string movieId)
    {
        if (Ratings == null)
        {
            Ratings = new List<UserRating>();
        }

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
        var couldNotFindException = new KeyNotFoundException(
            $"Tried to update rating from movie with id: {requestMovieId}, from user with id: {Id}, but could not find the rating"); 
        
        if (Ratings == null)
        {
            throw couldNotFindException;
        }

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

        throw couldNotFindException;
    }

    public int GetRatingForMovie(string movieId)
    {
        var couldNotFindException =  new KeyNotFoundException(
            $"Tried to get rating from movie with id: {movieId}, from user with id: {Id}, but could not find the rating");
        
        if (Ratings == null)
        {
            throw couldNotFindException;
        }

        foreach (var rating in Ratings)
        {
            if (rating.MovieId == movieId)
            {
                return rating.NumberOfStars;
            }
        }

        throw couldNotFindException;
    }
    
    public void SetRatingAvg()
    {
        var count = 0.0f;
        if (Ratings == null || !Ratings.Any())
        {
            AverageOfUserRatings = count;
            return;
        }


        foreach (var rating in Ratings)
        {
            count += rating.NumberOfStars;
        }
        
        AverageOfUserRatings = count / Ratings.Count;
    }
}