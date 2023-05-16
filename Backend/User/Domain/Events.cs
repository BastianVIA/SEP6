using MediatR;

namespace Backend.User.Domain;

public class UpdatedRatingEvent : INotification
{
    public string UserId { get; set; }
    public string MovieId { get; set; }
    public int? NewRating { get; set; }
}