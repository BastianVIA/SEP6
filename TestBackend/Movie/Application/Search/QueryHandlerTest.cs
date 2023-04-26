using Backend.Movie.Application.Search;
using Backend.Movie.Infrastructure;
using NSubstitute;

namespace TestBackend.Movie.Application.Search;

public class QueryHandlerTest
{
    
    [Fact]
    public async Task SearchForNonExistentMovieReturnsEmptyList()
    {
        var title = "MovieNameDoesNotExist";
        var mock = Substitute.For<IMovieRepository>();
        mock.SearchForMovie(title).Returns(new List<Backend.Movie.Domain.Movie>());

        var handler = new QueryHandler(mock);
        var request = new Query(title);
        var response = await handler.Handle(request, new CancellationToken());
        
        Assert.Empty(response.MovieDtos);
    }


    [Fact]
    public async Task SearchForOneMovieReturnsOneMovie()
    {
        var title = "Mario";
        var toReturn = new List<Backend.Movie.Domain.Movie>
        {
            new()
            {
                Id = 123456,
                ReleaseYear = 2001,
                Title = title
            }
        };
        
        var mock = Substitute.For<IMovieRepository>();
        mock.SearchForMovie(title).Returns(toReturn);

        var handler = new QueryHandler(mock);
        var request = new Query(title);
        var response = await handler.Handle(request, new CancellationToken());
        
        
        Assert.Single(response.MovieDtos);
        Assert.Equal(toReturn[0].Id, response.MovieDtos[0].Id);
        Assert.Equal(toReturn[0].ReleaseYear, response.MovieDtos[0].ReleaseYear);
        Assert.Equal(toReturn[0].Title, response.MovieDtos[0].Title);

    }

    [Fact]
    public async Task SearchForMovieReturnsCorrectListOrder()
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
        
        Assert.Equal(toReturn.Count, response.MovieDtos.Count);
        Assert.Equal(toReturn[0].Title, response.MovieDtos[0].Title);
        Assert.Equal(toReturn[1].Title, response.MovieDtos[1].Title);
        Assert.Equal(toReturn[2].Title, response.MovieDtos[2].Title);
    }

    [Fact]
    public async Task Noget()
    {
        var mock = Substitute.For<IMovieRepository>();
        
    }
}