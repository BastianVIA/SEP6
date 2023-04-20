using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.Search;

public record Query(string Title) : IRequest<MovieSearchResponse>;

public record MovieSearchResponse(List<MovieDto> movieDtos);

public class MovieDto
{
    public string Title { get; set; }
    public int Id { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }

}

public class QueryHandler : IRequestHandler<Query, MovieSearchResponse>
{
    private readonly IMovieRepository _repository;
    private readonly IImageService _imageService;

    public QueryHandler(IMovieRepository repository, IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }
    
    public async Task<MovieSearchResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var foundMovies = await _repository.SearchForMovie(request.Title);
        var moviesToDto = new List<MovieDto>();
        foreach (var foundMovie in foundMovies)
        {
            var posterPath = _imageService.GetPathForPoster(foundMovie.Id); 
            moviesToDto.Add(new MovieDto
            {
                Id = foundMovie.Id,
                Title = foundMovie.Title,
                ReleaseYear = foundMovie.ReleaseYear,
                PathToPoster = await posterPath
            });
        }

        return new MovieSearchResponse(moviesToDto);
    }
}