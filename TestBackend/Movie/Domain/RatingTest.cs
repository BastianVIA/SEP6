using System.ComponentModel.DataAnnotations;
using Backend.Movie.Domain;
using Xunit;

namespace TestBackend.Movie.Domain;


public class RatingTests
{
    private Rating rating;

    public RatingTests()
    {
        rating = new Rating();
    }

    [Fact]
    public void NewRating_AddValidRating_UpdatesAverageRatingAndVotes()
    {
        rating.NewRating(4);

        Assert.Equal(4, rating.AverageRating);
        Assert.Equal(1, rating.Votes);
    }

    [Fact]
    public void NewRating_AddInvalidRating_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() => rating.NewRating(11));
        Assert.Throws<ValidationException>(() => rating.NewRating(0));
    }

    [Fact]
    public void UpdateRating_ModifyExistingRating_UpdatesAverageRating()
    {
        rating.NewRating(3);
        rating.UpdateRating(3, 5);

        Assert.Equal(5, rating.AverageRating);
        Assert.Equal(1, rating.Votes);
    }

    [Fact]
    public void UpdateRating_UpdateWithInvalidRating_ThrowsValidationException()
    {
        rating.NewRating(3);

        Assert.Throws<ValidationException>(() => rating.UpdateRating(3, 11));
        Assert.Throws<ValidationException>(() => rating.UpdateRating(3, 0));
    }

    [Fact]
    public void RemoveRating_RemoveExistingRating_UpdatesAverageRatingAndVotes()
    {
        rating.NewRating(4);
        rating.RemoveRating(4);

        Assert.Equal(0, rating.AverageRating);
        Assert.Equal(0, rating.Votes);
    }

    [Fact]
    public void RemoveRating_RemoveRatingFromMultipleVotes_UpdatesAverageRatingAndVotes()
    {
        rating.NewRating(5);
        rating.NewRating(3);
        rating.NewRating(2);
        rating.RemoveRating(3);

        Assert.Equal(3.5f, rating.AverageRating);
        Assert.Equal(2, rating.Votes);
    }
}

