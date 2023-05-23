using Backend.SocialFeed.Domain;

namespace TestBackend.Social.Domain;

using Xunit;

public class PostTests
{
    [Fact]
    public void AddComment_AddsCommentToList()
    {
        // Arrange
        var post = new Post("user1", Activity.FavoriteMovie);
        var userId = "user2";
        var content = "Random text.";

        // Act
        post.AddComment(userId, content);

        // Assert
        Assert.NotNull(post.Comments);
        Assert.Single(post.Comments);
        Assert.Equal(userId, post.Comments[0].UserId);
        Assert.Equal(content, post.Comments[0].Contents);
    }

    [Fact]
    public void PutReaction_AddsReactionToDictionary()
    {
        // Arrange
        var post = new Post("user1", Activity.CreatedRating);
        var userId = "user2";
        var reaction = TypeOfReaction.LIKE;

        // Act
        post.PutReaction(userId, reaction);

        // Assert
        Assert.NotNull(post.Reactions);
        Assert.Single(post.Reactions);
        Assert.Equal(reaction, post.Reactions[userId]);
    }

    [Fact]
    public void PutReaction_UpdatesExistingReaction()
    {
        // Arrange
        var post = new Post("user1", Activity.CreatedRating);
        var userId = "user2";
        var initialReaction = TypeOfReaction.LIKE;
        var updatedReaction = TypeOfReaction.LIKE; // Same reaction, should remove it

        post.PutReaction(userId, initialReaction);

        // Act
        post.PutReaction(userId, updatedReaction);

        // Assert
        Assert.NotNull(post.Reactions);
        Assert.Empty(post.Reactions);
    }

    [Fact]
    public void GetUsersReaction_ReturnsReactionForValidUserId()
    {
        // Arrange
        var post = new Post("user1", Activity.CreatedReview);
        var userId = "user2";
        var reaction = TypeOfReaction.LIKE;

        post.PutReaction(userId, reaction);

        // Act
        var result = post.GetUsersReaction(userId);

        // Assert
        Assert.Equal(reaction, result);
    }

    [Fact]
    public void GetUsersReaction_ReturnsNullForInvalidUserId()
    {
        // Arrange
        var post = new Post("user1", Activity.CreatedReview);
        var userId = "user2";
        var reaction = TypeOfReaction.LIKE;

        post.PutReaction(userId, reaction);

        // Act
        var result = post.GetUsersReaction("invalidUserId");

        // Assert
        Assert.Null(result);
    }
}
