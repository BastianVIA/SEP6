using System.ComponentModel.DataAnnotations;
using Backend.User.Domain;

namespace TestBackend.User.Domain;

public class UserFavoriteMovieTests
{
    [Fact]
    public void Constructor_WithId_ThrowsValidationExceptionForEmptyMovieId()
    {
        // Arrange
        string movieId = "";

        // Act-Assert
        Assert.Throws<ValidationException>(() => new UserFavoriteMovie(movieId));
    }

}