using System.ComponentModel.DataAnnotations;
using Backend.Social.Domain;

namespace TestBackend.Social.Domain;

public class CommentTests
{
    
    [Fact]
    public void Comment_Constructor_CreatesCommentIfValid()
    {
        // Arrange
        var userId = "user1";
        var content = "Real comment";

        // Act
        var comment = new Comment(userId, content);
        // Assert
        Assert.NotEqual(Guid.Empty, comment.Id);
        Assert.Equal(content, comment.Contents);
        Assert.Equal(userId, comment.UserId);
        Assert.True(comment.TimeStamp <= DateTime.Now);
    }
    [Fact]
    public void Comment_Constructor_ThrowsValidationExceptionForEmptyUserId()
    {
        // Arrange
        var userId = "";
        var content = "Real comment";

        // Act-Assert
        Assert.Throws<ValidationException>(() => new Comment(userId, content));
    }

    [Fact]
    public void Comment_Constructor_ThrowsValidationExceptionForEmptyContent()
    {
        // Arrange
        var userId = "user1";
        var content = ""; 

        // Act-Assert
        Assert.Throws<ValidationException>(() => new Comment(userId, content));
    }
}
