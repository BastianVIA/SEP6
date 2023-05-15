using Backend.Movie.Application.Search;
using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetMovieInfo;

public record Query(string Id) : IRequest<MovieInfoDto>;

public class MovieInfoDto
{
    public string Id { get; set; }
    public string  Title { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoser { get; set; }
    public RatingDto? Rating { get; set; }
}

public record RatingDto(float AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, MovieInfoDto>
{
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;

    public QueryHandler(IMovieRepository repository, IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }

    public async Task<MovieInfoDto> Handle(Query request, CancellationToken cancellationToken)
    {
        var movie = await _repository.ReadMovieFromId(request.Id);
        var image = await _imageService.GetPathForPoster(movie.Id);

        return toDto(movie, image);
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
            movieDto.Rating = new RatingDto(movie.Rating.AverageRating, movie.Rating.Votes);
        }
        return movieDto;
    }
}