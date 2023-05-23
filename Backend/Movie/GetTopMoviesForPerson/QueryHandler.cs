using System.Diagnostics;
using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.GetTopMoviesForPerson;

public record Query(string PersonId):IRequest<GetTopMoviesForPersonResponse>;

public record GetTopMoviesForPersonResponse(List<TopMoviesDto>? ActedMovies, List<TopMoviesDto>? DirectedMovies);

public class TopMoviesDto
{
    public string MovieId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
}

public class QueryHandler : IRequestHandler<Query, GetTopMoviesForPersonResponse>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;

    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IMovieRepository repository, IImageService imageService)
    {
        _transactionFactory = transactionFactory;
        _repository = repository;
        _imageService = imageService;
    }

    public async Task<GetTopMoviesForPersonResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        Stopwatch stopwatchAct = Stopwatch.StartNew();
        var actedMovies =  await _repository.GetActedMoviesForPersonAsync(request.PersonId, transaction);
        stopwatchAct.Stop();
        Console.WriteLine("--------- ACT "+ stopwatchAct.Elapsed);

        Stopwatch stopwatchDirect = Stopwatch.StartNew();
        var directedMovies =  await _repository.GetDirectedMoviesForPersonAsync(request.PersonId, transaction);
        
        stopwatchDirect.Stop();
        Console.WriteLine("--------- DIRECT "+ stopwatchDirect.Elapsed);
        
        Stopwatch stopwatchDto = Stopwatch.StartNew();
        var dto = await toDto(actedMovies, directedMovies);
        stopwatchDto.Stop();
        Console.WriteLine("--------- DTO "+ stopwatchDto.Elapsed);
        return dto;
    }

    private async Task<GetTopMoviesForPersonResponse> toDto(List<Domain.Movie>? actedMovies, List<Domain.Movie>? directedMovies)
    {
        return new GetTopMoviesForPersonResponse(await toDto(actedMovies),await toDto(directedMovies));
    }

    private async Task<List<TopMoviesDto>?> toDto(List<Domain.Movie>? movies)
    {
        if (movies == null || !movies.Any())
        {
            return null;
        }

        var movieList = new List<TopMoviesDto>();
        foreach (var m in movies)
        {
            var pathToPoster = _imageService.GetPathForPosterAsync(m.Id);
            movieList.Add(new TopMoviesDto
            {
                MovieId = m.Id,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                PathToPoster = await pathToPoster
            });
        }

        return movieList;
    }
}