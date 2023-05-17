using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetInfoFromMovies;

public record Query(List<string> movieIds, int requestedPageNumber = 1) : IRequest<MoviesInfoResponse>;

public record MoviesInfoResponse(List<MovieInfoDto> MovieInfoDtos);

public class MovieInfoDto
{
    public string Id { get; set; }
    public string  Title { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoser { get; set; }
    public MovieInfoRatingDto? Rating { get; set; }
}

public record MovieInfoRatingDto(float AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, MoviesInfoResponse>
{
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IMovieRepository repository, IImageService imageService, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _databaseTransactionFactory = databaseTransactionFactory;
    }
    
    public async Task<MoviesInfoResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var listOfMovies = _repository.ReadMoviesFromList(request.movieIds, request.requestedPageNumber, transaction);
        var listOfMoviesAsDto = new List<MovieInfoDto>();
        foreach (var movie in await listOfMovies)
        {
            var pathToPoster = _imageService.GetPathForPoster(movie.Id);
            listOfMoviesAsDto.Add(toDto(movie, await pathToPoster));
        }

        return new MoviesInfoResponse(listOfMoviesAsDto);
    }
    
    private MovieInfoDto toDto(Domain.Movie movie, Uri? imagePath)
    {
        var movieDto = new MovieInfoDto
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PathToPoser = imagePath
        };
        if (movie.Rating != null)
        {
            movieDto.Rating = new MovieInfoRatingDto(movie.Rating.AverageRating, movie.Rating.Votes);
        }
        return movieDto;
    }
}