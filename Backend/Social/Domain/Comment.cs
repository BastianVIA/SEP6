namespace Backend.SocialFeed.Domain;

public class Comment
{
    public Guid Id  { get; set; }
    public string UserId { get; set; }
    public string Contents { get; set; }
}