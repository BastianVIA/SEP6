using Frontend.Service;

namespace Frontend.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string? ProfilePicture { get; set;}
    public int NumberOfReactions { get; set; }
    public string PostText { get; set; }
    public string TimeSincePostText { get; set; }
    public ActivityData? ActivityData { get; set; }
    public List<Comment>? Comments { get; set; }
    
}