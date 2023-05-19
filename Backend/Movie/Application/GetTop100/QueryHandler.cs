using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetTop100;

public record Query() : IRequest<MovieTop100Response>;

public record MovieTop100Response(List<MovieTop100Dto> topMovies);

public class MovieTop100Dto
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public MovieTop100RatingDto? Rating { get; set; }
}

public record MovieTop100RatingDto(float AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, MovieTop100Response>
{
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    private int MinVotesForQuery;

    public QueryHandler(IConfiguration configuration, IMovieRepository repository, IImageService imageService,
        IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _transactionFactory = transactionFactory;
        MinVotesForQuery = configuration.GetSection("QueryConstants").GetValue<int>("MinVotes");
    }


    public async Task<MovieTop100Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var movies = await _repository.GetTop100Movies(
            MinVotesForQuery, 
            transaction);

        var dtoMovies = new List<MovieTop100Dto>();
        foreach (var domainMovie in movies)
        {
            var posterForMovie = _imageService.GetPathForPoster(domainMovie.Id);
            var dtoMovie = new MovieTop100Dto
            {
                Id = domainMovie.Id,
                Title = domainMovie.Title,
                ReleaseYear = domainMovie.ReleaseYear,
                PathToPoster = await posterForMovie
            };
            if (domainMovie.Rating != null)
            {
                dtoMovie.Rating = new MovieTop100RatingDto(domainMovie.Rating.AverageRating, domainMovie.Rating.Votes);
            }
            dtoMovies.Add(dtoMovie);
        }
        return new MovieTop100Response(dtoMovies);
    }
}
