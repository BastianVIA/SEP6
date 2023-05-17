using System.Net.NetworkInformation;
using AutoFixture;
using Backend.Movie.Application.GetDetails;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace TestBackend.Movie.Application.Details;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IResumeService _resumeService = Substitute.For<IResumeService>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _imageService, _resumeService, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMovieKeyNotFound()
    {
        //Arrange
        var query = _fixture.Create<Query>();
        _repository.ReadMovieFromId(query.Id).Throws<KeyNotFoundException>();
        //Act-Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(()=>_handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnMovie_WhenIdFound()
    {
        //Arrange
        var query = _fixture.Create<Query>();
        var expectedMovie = _fixture.Create<Backend.Movie.Domain.Movie>();
        var expectedPoster = _fixture.Create<Uri>();
        var expectedResume = _fixture.Create<string>();
        
        _repository.ReadMovieFromId(query.Id).Returns(expectedMovie);
        _imageService.GetPathForPoster(query.Id).Returns(expectedPoster);
        _resumeService.GetResume(query.Id).Returns(expectedResume);
        
        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert
        Assert.Equal(expectedMovie.Id, result.MovieDetailsDto.Id);
        Assert.Equal(expectedMovie.Title, result.MovieDetailsDto.Title);
        Assert.Equal(expectedMovie.ReleaseYear, result.MovieDetailsDto.ReleaseYear);
        Assert.Equal(expectedMovie.Rating.AverageRating, result.MovieDetailsDto.Ratings.AverageRating);
        Assert.Equal(expectedMovie.Rating.Votes, result.MovieDetailsDto.Ratings.NumberOfVotes);
        Assert.Equivalent(expectedMovie.Actors, result.MovieDetailsDto.Actors);
        Assert.Equivalent(expectedMovie.Directors, result.MovieDetailsDto.Directors);
        Assert.Equal(expectedPoster, result.MovieDetailsDto.PathToPoster);
        Assert.Equal(expectedResume, result.MovieDetailsDto.Resume);
        
    }
    
    // [Fact]
    // public async Task Handle_ShouldReturnMovie_WhenIdFound()
    // {
    //     //Arrange
    //     var query = _fixture.Create<Query>();
    //     var expected = _fixture.Create<Backend.Movie.Domain.Movie>();
    //     _repository.ReadMovieFromId(query.Id).Returns(expected);
    //     
    //     //Act
    //
    //     var result = await _sut.Handle(query, CancellationToken.None);
    //     //Assert
    //     Assert.Equal(expected.Id, result.MovieDetailsDto.Id);
    //     Assert.Equal(expected.Title, result.MovieDetailsDto.Title);
    //     Assert.Equal(expected.ReleaseYear, result.MovieDetailsDto.ReleaseYear);
    //     Assert.Equal(expected.Rating.AverageRating, result.MovieDetailsDto.Ratings.AverageRating);
    //     Assert.Equal(expected.Rating.Votes, result.MovieDetailsDto.Ratings.NumberOfVotes);
    // }
}