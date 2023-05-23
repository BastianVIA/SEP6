namespace TestBackend.Movie.Domain;
using System.Collections.Generic;
using Xunit;

public class MovieTests
{
    private Backend.Movie.Domain.Movie movie;

    public MovieTests()
    {
        movie = new Backend.Movie.Domain.Movie();
    }

    [Fact]
    public void NewRating_AddNewRating_UpdatesMovieRating()
    {
        movie.NewRating(4);

        Assert.NotNull(movie.Rating);
        Assert.Equal(4, movie.Rating.AverageRating);
        Assert.Equal(1, movie.Rating.Votes);
    }

    [Fact]
    public void UpdateRating_UpdateExistingRating_UpdatesMovieRating()
    {
        movie.NewRating(3);
        movie.UpdateRating(3, 5);

        Assert.Equal(5, movie.Rating.AverageRating);
        Assert.Equal(1, movie.Rating.Votes);
    }

    [Fact]
    public void UpdateRating_UpdateRatingWithoutExistingRating_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => movie.UpdateRating(3, 5));
    }

    [Fact]
    public void RemoveRating_RemoveExistingRating_UpdatesMovieRating()
    {
        movie.NewRating(4);
        movie.RemoveRating(4);

        Assert.Equal(0, movie.Rating.Votes);
    }

    [Fact]
    public void RemoveRating_RemoveRatingWithoutExistingRating_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => movie.RemoveRating(4));
    }
}
