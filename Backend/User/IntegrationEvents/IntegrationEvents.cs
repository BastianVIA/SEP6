using MediatR;

namespace Backend.User.IntegrationEvents;

public class UserCreatedIntegrationEvent : INotification
{
    public string UserId { get; }

    public UserCreatedIntegrationEvent(string userId)
    {
        UserId = userId;
    }
}

public class UnFavoritedMovieIntegrationEvent : INotification
{
    public UnFavoritedMovieIntegrationEvent(string userId, string movieId)
    {
        UserId = userId;
        MovieId = movieId;
    }

    public string UserId { get; }
    public string MovieId { get; }
}

public class FavoritedMovieIntegrationEvent : INotification
{
    public string UserId { get; }
    public string MovieId { get; }

    public FavoritedMovieIntegrationEvent(string userId, string movieId)
    {
        UserId = userId;
        MovieId = movieId;
    }
}

public class CreatedRatingIntegrationEvent : INotification
{
    public string UserId { get; }
    public string MovieId { get; }
    public int Rating { get; }

    public CreatedRatingIntegrationEvent(string userId, string movieId, int rating)
    {
        UserId = userId;
        MovieId = movieId;
        Rating = rating;
    }
}

public class RemovedRatingIntegrationEvent : INotification
{
    public string UserId { get; }
    public string MovieId { get; }
    public int PreviousRating { get; }

    public RemovedRatingIntegrationEvent(string userId, string movieId, int previousRating)
    {
        UserId = userId;
        MovieId = movieId;
        PreviousRating = previousRating;
    }
}

public class UpdatedRatingIntegrationEvent : INotification
{
    public string UserId { get; }
    public string MovieId { get; }
    public int NewRating { get; }
    public int PreviousRating { get; }

    public UpdatedRatingIntegrationEvent(string userId, string movieId, int previousRating, int newRating)
    {
        UserId = userId;
        MovieId = movieId;
        PreviousRating = previousRating;
        NewRating = newRating;
    }
}

public class CreatedReviewIntegrationEvent : INotification
{
    public string UserId { get; set; }
    public string MovieId { get; set; }
    public string ReviewBody { get; set; }

    public CreatedReviewIntegrationEvent(string userId, string movieId, string reviewBody)
    {
        UserId = userId;
        MovieId = movieId;
        ReviewBody = reviewBody;
    }
}