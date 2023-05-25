using System.ComponentModel.DataAnnotations;
using Backend.Social.Domain;

namespace TestBackend.Social.Domain;

public class SocialUserTests
{
    [Fact]
    public void Constructor_WithId_AddsSocialUserCreatedEvent()
    {
        // Arrange
        string userId = "user123";

        // Act
        var socialUser = new SocialUser(userId);

        // Assert
        var domainEvents = socialUser.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(SocialUserCreatedEvent), domainEvents.First().GetType());
        Assert.Equal(userId, ((SocialUserCreatedEvent)domainEvents.First()).Id);
    }
    [Fact]
    public void Constructor_WithId_ThrowsValidationExceptionForEmptyUserId()
    {
        // Arrange
        string userId = "";

        // Act-Assert
        Assert.Throws<ValidationException>(() => new SocialUser(userId));
    }

    [Fact]
    public void Constructor_WithoutId_DoesNotAddUserCreatedEvent()
    {
        // Arrange
        string userId = "user123";

        // Act
        var socialUser = new SocialUser
        {
            Id = userId
        };

        // Assert
        var domainEvents = socialUser.ReadAllDomainEvents();
        Assert.Empty(domainEvents);
    }

    [Fact]
    public void AlreadyFollows_UserNotInFollowing_ReturnsFalse()
    {
        // Arrange
        var socialUser = new SocialUser();

        // Act
        bool result = socialUser.AlreadyFollows("user123");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AlreadyFollows_UserInFollowing_ReturnsTrue()
    {
        // Arrange
        var socialUser = new SocialUser();
        socialUser.StartFollowing("user123");

        // Act
        bool result = socialUser.AlreadyFollows("user123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StartFollowing_AddUserToFollowingList()
    {
        // Arrange
        var socialUser = new SocialUser();

        // Act
        socialUser.StartFollowing("user123");

        // Assert
        Assert.Single(socialUser.Following);
        Assert.Equal("user123", socialUser.Following.First());
    }
}
