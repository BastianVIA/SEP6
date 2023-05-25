using System.ComponentModel.DataAnnotations;

namespace Backend.Social.Domain;

public class Comment
{
    public Guid Id  { get; set; }
    public string UserId { get; set; }
    public string Contents { get; set; }
    public DateTime TimeStamp { get; set; }
    
    public Comment(){}

    public Comment(string userId, string content)
    {
        if (userId == "")
        {
            throw new ValidationException(
                $"Trying to add a Comment with empty userId, this is not allowed, a Comment has to have a userId");
        }

        if (content == "")
        {
            throw new ValidationException(
                $"Trying to add a Comment with empty content, this is not allowed, a Comment has to have content"); 
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Contents = content;
        TimeStamp = DateTime.Now;
    }
}