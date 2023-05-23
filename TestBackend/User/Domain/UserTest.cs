using System.ComponentModel.DataAnnotations;
using Backend.User.Domain;

namespace TestBackend.User.Domain;

using System;
using System.Collections.Generic;
using Xunit;

public class UserTests
{
    [Fact]
    public void Constructor_WithIdDisplayNameEmail_AddsUserCreatedEvent()
    {
        // Arrange
        string userId = "user123";
        string displayName = "John Doe";
        string email = "john.doe@example.com";

        // Act
        var user = new Backend.User.Domain.User(userId, displayName, email);

        // Assert
        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(UserCreated), domainEvents.First().GetType());
        Assert.Equal(userId, ((UserCreated)domainEvents.First()).UserId);
    }

    [Fact]
    public void HasAlreadyFavoritedMovie_UserHasNotFavoritedMovie_ReturnsFalse()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act
        bool result = user.HasAlreadyFavoritedMovie("movie123");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasAlreadyFavoritedMovie_UserHasFavoritedMovie_ReturnsTrue()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddFavoriteMovie("movie123");

        // Act
        bool result = user.HasAlreadyFavoritedMovie("movie123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddRating_AddNewRating_AddsUserRatingAndCreatedRatingEvent()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        string movieId = "movie123";
        int rating = 4;

        // Act
        user.AddRating(movieId, rating);

        // Assert
        Assert.Single(user.Ratings);
        Assert.Equal(movieId, user.Ratings[0].MovieId);
        Assert.Equal(rating, user.Ratings[0].NumberOfStars);

        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(CreatedRatingEvent), domainEvents.First().GetType());
        var createdRatingEvent = (CreatedRatingEvent)domainEvents.First();
        Assert.Equal(user.Id, createdRatingEvent.UserId);
        Assert.Equal(movieId, createdRatingEvent.Rating.MovieId);
        Assert.Equal(rating, createdRatingEvent.Rating.NumberOfStars);
    }

    [Fact]
    public void RemoveFavorite_UserHasFavoritedMovie_RemovesFavoriteAndAddsUnfavoritedMovieEvent()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddFavoriteMovie("movie123");
        user.ReadAllDomainEvents(); //To clear the list of domain events

        // Act
        user.RemoveFavorite("movie123");

        // Assert
        Assert.Empty(user.FavoriteMovies);

        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(UnFavoritedMovie), domainEvents.First().GetType());
        var unfavoritedMovieEvent = (UnFavoritedMovie)domainEvents.First();
        Assert.Equal(user.Id, unfavoritedMovieEvent.UserId);
        Assert.Equal("movie123", unfavoritedMovieEvent.MovieId);
    }

    [Fact]
    public void RemoveFavorite_UserHasNotFavoritedMovie_ThrowsValidationException()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act & Assert
        Assert.Throws<ValidationException>(() => user.RemoveFavorite("movie123"));
    }

    [Fact]
    public void HasAlreadyRatedMovie_UserHasNotRatedMovie_ReturnsFalse()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act
        bool result = user.HasAlreadyRatedMovie("movie123");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasAlreadyRatedMovie_UserHasRatedMovie_ReturnsTrue()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddRating("movie123", 4);

        // Act
        bool result = user.HasAlreadyRatedMovie("movie123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddFavoriteMovie_AddNewFavorite_AddsMovieToFavoriteMoviesAndAddsFavoritedMovieEvent()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        string movieId = "movie123";

        // Act
        user.AddFavoriteMovie(movieId);

        // Assert
        Assert.Single(user.FavoriteMovies);
        Assert.Equal(movieId, user.FavoriteMovies[0]);

        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(FavoritedMovie), domainEvents.First().GetType());
        var favoritedMovieEvent = (FavoritedMovie)domainEvents.First();
        Assert.Equal(user.Id, favoritedMovieEvent.UserId);
        Assert.Equal(movieId, favoritedMovieEvent.MovieId);
    }

    [Fact]
    public void RemoveRating_UserHasRatedMovie_RemovesRatingAndAddsRemovedRatingEvent()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddRating("movie123", 4);
        user.ReadAllDomainEvents(); //To clear the list of domain events

        // Act
        user.RemoveRating("movie123");

        // Assert
        Assert.Empty(user.Ratings);

        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(RemovedRatingEvent), domainEvents.First().GetType());
        var removedRatingEvent = (RemovedRatingEvent)domainEvents.First();
        Assert.Equal(user.Id, removedRatingEvent.UserId);
        Assert.Equal("movie123", removedRatingEvent.Rating.MovieId);
        Assert.Equal(4, removedRatingEvent.Rating.NumberOfStars);
    }

    [Fact]
    public void RemoveRating_UserHasNotRatedMovie_ThrowsKeyNotFoundException()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => user.RemoveRating("movie123"));
    }

    [Fact]
    public void UpdateRating_UserHasRatedMovie_UpdatesRatingAndAddsUpdatedRatingEvent()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddRating("movie123", 4);
        string movieId = "movie123";
        int newRating = 5;
        user.ReadAllDomainEvents(); //To clear the list of domain events

        // Act
        user.UpdateRating(movieId, newRating);

        // Assert
        Assert.Single(user.Ratings);
        Assert.Equal(movieId, user.Ratings[0].MovieId);
        Assert.Equal(newRating, user.Ratings[0].NumberOfStars);

        var domainEvents = user.ReadAllDomainEvents();
        Assert.Single(domainEvents);
        Assert.Equal(typeof(UpdatedRatingEvent), domainEvents.First().GetType());
        var updatedRatingEvent = (UpdatedRatingEvent)domainEvents.First();
        Assert.Equal(user.Id, updatedRatingEvent.UserId);
        Assert.Equal(movieId, updatedRatingEvent.MovieId);
        Assert.Equal(4, updatedRatingEvent.PreviousRating);
        Assert.Equal(newRating, updatedRatingEvent.NewRating);
    }

    [Fact]
    public void UpdateRating_UserHasNotRatedMovie_ThrowsKeyNotFoundException()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => user.UpdateRating("movie123", 5));
    }

    [Fact]
    public void GetRatingForMovie_UserHasRatedMovie_ReturnsRating()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddRating("movie123", 4);

        // Act
        int rating = user.GetRatingForMovie("movie123");

        // Assert
        Assert.Equal(4, rating);
    }

    [Fact]
    public void GetRatingForMovie_UserHasNotRatedMovie_ThrowsKeyNotFoundException()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => user.GetRatingForMovie("movie123"));
    }

    [Fact]
    public void SetRatingAvg_UserHasNoRatings_SetsAverageOfUserRatingsToZero()
    {
        // Arrange
        var user = new Backend.User.Domain.User();

        // Act
        user.SetRatingAvg();

        // Assert
        Assert.Equal(0.0f, user.AverageOfUserRatings);
    }

    [Fact]
    public void SetRatingAvg_UserHasRatings_SetsAverageOfUserRatings()
    {
        // Arrange
        var user = new Backend.User.Domain.User();
        user.AddRating("movie123", 4);
        user.AddRating("movie456", 5);

        // Act
        user.SetRatingAvg();

        // Assert
        Assert.Equal(4.5f, user.AverageOfUserRatings);
    }
}
