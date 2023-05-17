using MediatR;

namespace Backend.User.Domain;

public class CreatedRatingEvent : INotification
{
    public string UserId { get;  }
    public UserRating Rating { get; }

    public CreatedRatingEvent(string userId, UserRating rating)
    {
        UserId = userId;
        Rating = rating;
    }
}

public class RemovedRatingEvent : INotification
{
    public string UserId { get;  }
    public UserRating Rating { get; }
    public RemovedRatingEvent(string userId, UserRating rating)
    {
        UserId = userId;
        Rating = rating;
    }
}

public class UpdatedRatingEvent : INotification
{
    public string UserId { get;  }
    public string MovieId { get;  }
    public int NewRating { get; }
    public int PreviousRating { get; }

    public UpdatedRatingEvent(string userId, string movieId, int previousRating, int newRating)
    {
        UserId = userId;
        MovieId = movieId;
        PreviousRating = previousRating;
        NewRating = newRating;
    }
}