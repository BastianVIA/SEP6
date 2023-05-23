using System.Net.NetworkInformation;
using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetDetails;
using Backend.Movie.Domain;
using Backend.Movie.Infrastructure;
using Backend.People.Application.GetPeopleFromId;
using Backend.People.Application.Search;
using Backend.User.Application.GetUserInfoForMovie;
using Backend.Service;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Query = Backend.Movie.Application.GetDetails.Query;
using QueryHandler = Backend.Movie.Application.GetDetails.QueryHandler;

namespace TestBackend.Movie.Application.Details;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IResumeService _resumeService = Substitute.For<IResumeService>();
    private readonly ITrailerService _trailerService = Substitute.For<ITrailerService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _imageService, _resumeService, _trailerService, _mediator, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMovieKeyNotFound()
    {
        //Arrange
        var query = _fixture.Create<Query>();
        _repository.ReadMovieFromId(query.Id, Arg.Any<DbReadOnlyTransaction>(),  Arg.Any<bool>(), Arg.Any<bool>(),
            Arg.Any<bool>()).Throws<KeyNotFoundException>();
        //Act-Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnMovie_WhenIdFound()
    {
        //Arrange
        var query = _fixture.Create<Query>();
        var expectedMovie2 = _fixture.Create<Backend.Movie.Domain.Movie>();
        var expectedPoster = _fixture.Create<Uri>();
        var expectedResume = _fixture.Create<string>();
        var expectedUserInfoForMovie = _fixture.Create<GetUserInfoForMovieResponse>();


        List<PersonDto> actorDtoList = new List<PersonDto>();
        List<PersonDto> directorDtoList = new List<PersonDto>();
        AddPeopleToList(expectedMovie2.Actors, actorDtoList);
        AddPeopleToList(expectedMovie2.Directors, directorDtoList);
        
        _repository.ReadMovieFromId(query.Id, Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>(), Arg.Any<bool>(),
            Arg.Any<bool>()).Returns(expectedMovie2);
        _imageService.GetPathForPoster(query.Id).Returns(expectedPoster);
        _resumeService.GetResume(query.Id).Returns(expectedResume);
        _mediator.Send(Arg.Any<Backend.User.Application.GetUserInfoForMovie.Query>()).Returns(expectedUserInfoForMovie);
        _mediator.Send(Arg.Is(new Backend.People.Application.GetPeopleFromId.Query(expectedMovie2.Actors)))
            .Returns(new PersonResponse(actorDtoList));
        _mediator.Send(Arg.Is(new Backend.People.Application.GetPeopleFromId.Query(expectedMovie2.Directors)))
            .Returns(new PersonResponse(directorDtoList));

        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert
        Assert.Equal(expectedMovie2.Id, result.MovieDetailsDto.Id);
        Assert.Equal(expectedMovie2.Title, result.MovieDetailsDto.Title);
        Assert.Equal(expectedMovie2.ReleaseYear, result.MovieDetailsDto.ReleaseYear);
        Assert.Equal(expectedMovie2.Rating.AverageRating, result.MovieDetailsDto.Ratings.AverageRating);
        Assert.Equal(expectedMovie2.Rating.Votes, result.MovieDetailsDto.Ratings.NumberOfVotes);
        Assert.Equal(expectedPoster, result.MovieDetailsDto.PathToPoster);
        Assert.Equal(expectedResume, result.MovieDetailsDto.Resume);

        if (expectedMovie2.Actors != null)
        {
            var movieDetailsActor = result.MovieDetailsDto.Actors.Select(a => a.Id).ToList();
            Assert.Equivalent(expectedMovie2.Actors, movieDetailsActor);
        }

        if (expectedMovie2.Directors != null)
        {
            var movieDetailsDirector = result.MovieDetailsDto.Directors.Select(a => a.Id).ToList();
            Assert.Equivalent(expectedMovie2.Directors, movieDetailsDirector);
        }
    }

    private static void AddPeopleToList(List<string>? expectedMovieList, List<PersonDto> personDtoList)
    {
        if (expectedMovieList != null)
        {
            foreach (var personId in expectedMovieList)
            {
                personDtoList.Add(new PersonDto
                {
                    ID = personId,
                    Name = "Person Name",
                    BirthYear = 2021
                });
            }
        }
    }
}