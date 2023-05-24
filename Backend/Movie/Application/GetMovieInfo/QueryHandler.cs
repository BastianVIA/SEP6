using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetMovieInfo;

public record Query(string MovieId) : IRequest<GetMovieInfoResponse>;

public class GetMovieInfoResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public GetMovieInfoRatings? Rating { get; set; }
}

public record GetMovieInfoRatings(double AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, GetMovieInfoResponse>
{
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IMovieRepository repository, IImageService imageService,
        IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<GetMovieInfoResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var pathForPoster = _imageService.GetPathForPosterAsync(request.MovieId);
        var movie = await _repository.ReadMovieFromIdAsync(request.MovieId, transaction);
        
        return await toDto(movie, pathForPoster);;
    }

    private static async Task<GetMovieInfoResponse> toDto(Domain.Movie movie, Task<Uri?> pathForPoster)
    {
        var response = new GetMovieInfoResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PathToPoster = await pathForPoster
        };
        if (movie.Rating != null)
        {
            response.Rating = new GetMovieInfoRatings(movie.Rating.AverageRating, movie.Rating.Votes);
        }

        return response;
    }
}