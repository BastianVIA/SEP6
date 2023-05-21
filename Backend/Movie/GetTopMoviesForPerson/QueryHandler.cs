using System.Diagnostics;
using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using MediatR;

namespace Backend.Movie.GetTopMoviesForPerson;

public record Query(string PersonId):IRequest<GetTopMoviesForPersonResponse>;

public record GetTopMoviesForPersonResponse(List<TopMoviesDto>? ActedMovies, List<TopMoviesDto>? DirectedMovies);

public class TopMoviesDto
{
    public string MovieId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
}

public class QueryHandler : IRequestHandler<Query, GetTopMoviesForPersonResponse>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IMovieRepository _repository;

    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IMovieRepository repository)
    {
        _transactionFactory = transactionFactory;
        _repository = repository;
    }

    public async Task<GetTopMoviesForPersonResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var actedMovies = _repository.GetActedMoviesForPerson(request.PersonId, transaction);
        var directedMovies = _repository.GetDirectedMoviesForPerson(request.PersonId, transaction);

        return toDto(await actedMovies, await directedMovies);
    }

    private GetTopMoviesForPersonResponse toDto(List<Domain.Movie>? actedMovies, List<Domain.Movie>? directedMovies)
    {
        return new GetTopMoviesForPersonResponse(toDto(actedMovies), toDto(directedMovies));
    }

    private List<TopMoviesDto>? toDto(List<Domain.Movie>? movies)
    {
        if (movies == null || !movies.Any())
        {
            return null;
        }

        var movieList = new List<TopMoviesDto>();
        foreach (var m in movies)
        {
            movieList.Add(new TopMoviesDto
            {
                MovieId = m.Id,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
            });
        }

        return movieList;
    }
}