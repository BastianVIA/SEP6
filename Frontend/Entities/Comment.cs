namespace Frontend.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string DisplayNameOfUser { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
}