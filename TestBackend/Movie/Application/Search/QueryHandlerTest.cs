using Backend.Movie.Application.Search;
using Backend.Movie.Infrastructure;
using NSubstitute;

namespace TestBackend.Movie.Application.Search;

public class QueryHandlerTest
{

    public async Task SearchForNonExistentMovieReturnsEmptyList()
    {
        var title = "MovieNameDoesNotExist";
        var mock = Substitute.For<IMovieRepository>();
        mock.SearchForMovie(title).Returns(new List<Backend.Movie.Domain.Movie>());

        var handler = new QueryHandler(mock);
        var request = new Query(title);
        var response = await handler.Handle(request, new CancellationToken());
        
        Assert.Empty(response.movieDtos);
    }


    [Fact]
    public async Task SearchForOneMovieReturnsOneMovie()
    {
        var title = "Mario";
        var id = 123456;
        var releaseYear = 2001;
        var testMovie = new Backend.Movie.Domain.Movie()
        {
            Id = id,
            ReleaseYear = releaseYear,
            Title = title
        };
        var mock = Substitute.For<IMovieRepository>();
        mock.SearchForMovie(title).Returns(new List<Backend.Movie.Domain.Movie>
        {
            testMovie
        });

        var handler = new QueryHandler(mock);
        var request = new Query(title);
        var response = await handler.Handle(request, new CancellationToken());
        
        Assert.Single(response.movieDtos);
        Assert.Equal(id, response.movieDtos[0].Id);
        Assert.Equal(releaseYear, response.movieDtos[0].ReleaseYear);
        Assert.Equal(title, response.movieDtos[0].Title);

    }

    [Fact]
    public async Task SearchForMovieReturnsManyMovies()
    {
        var title = "Mario";
        var toReturn = new List<Backend.Movie.Domain.Movie>()
        {
            new()
            {
                Title = $"{title}1",
                Id = 123456,
                ReleaseYear = 2002
            },
            new()
            {
                Title = $"{title}2",
                Id = 223456,
                ReleaseYear = 2004
            },
            new()
            {
                Title = $"{title}3",
                Id = 323456,
                ReleaseYear = 2005
            }
        };

        var mock = Substitute.For<IMovieRepository>();
        mock.SearchForMovie(title).Returns(toReturn);

        var handler = new QueryHandler(mock);
        var request = new Query(title);
        var response = await handler.Handle(request, new CancellationToken());
        
        Assert.Equal(toReturn.Count, response.movieDtos.Count);
    }

    [Fact]
    public async Task Noget()
    {
        var mock = Substitute.For<IMovieRepository>();
        
    }
}