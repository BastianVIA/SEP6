using System.ComponentModel.DataAnnotations;
using Backend.User.Domain;

namespace TestBackend.User.Domain;

public class UserRatingTests
{
    [Fact]
    public void UserRating_Constructor_DoesNotThrowWithValidData()
    {
        // Arrange
        var movieId = "movieId"; 
        var numberOfStars1 = 1;
        var numberOfStars10 = 10;
        var numberOfStars6 = 6;

        // Act
        var rating1 = new UserRating(movieId, numberOfStars1);
        var rating10 = new UserRating(movieId, numberOfStars10);
        var rating6 = new UserRating(movieId, numberOfStars6);
        
        // Assert
        Assert.Equal(rating1.MovieId, movieId);
        Assert.Equal(rating1.NumberOfStars, numberOfStars1);
        Assert.Equal(rating10.NumberOfStars, numberOfStars10);
        Assert.Equal(rating6.NumberOfStars, numberOfStars6);
    }
    [Fact]
    public void UserRating_Constructor_ThrowsValidationExceptionForEmptyMovieId()
    {
        // Arrange
        var movieId = ""; // Empty movieId
        var numberOfStars = 5;

        // Act and Assert
        Assert.Throws<ValidationException>(() => new UserRating(movieId, numberOfStars));
    }

    [Fact]
    public void UserRating_Constructor_ThrowsValidationExceptionForBelowZeroStars()
    {
        // Arrange
        var movieId = "movie1";
        var numberOfStars = 0;

        // Act and Assert
        Assert.Throws<ValidationException>(() => new UserRating(movieId, numberOfStars));
    }
    
    [Fact]
    public void UserRating_Constructor_ThrowsValidationExceptionForAboveTenStars()
    {
        // Arrange
        var movieId = "movie1";
        var numberOfStars = 11;

        // Act and Assert
        Assert.Throws<ValidationException>(() => new UserRating(movieId, numberOfStars));
    }
    
    
}