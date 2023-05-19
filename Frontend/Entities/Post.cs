using Frontend.Service;

namespace Frontend.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string? ProfilePicture { get; set;}
    public string PostText { get; set; }
    public string TimeSincePostText { get; set; }
    public ActivityData? ActivityData { get; set; }
}