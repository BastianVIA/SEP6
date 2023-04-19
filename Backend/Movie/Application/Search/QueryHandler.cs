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

}

public class QueryHandler : IRequestHandler<Query, MovieSearchResponse>
{
    private IMovieRepository _repository;

    public QueryHandler(IMovieRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<MovieSearchResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var foundMovies = await _repository.SearchForMovie(request.Title);
        var moviesToDto = new List<MovieDto>();
        foreach (var foundMovie in foundMovies)
        {
            moviesToDto.Add(new MovieDto
            {
                Id = foundMovie.Id,
                Title = foundMovie.Title,
                ReleaseYear = foundMovie.ReleaseYear
            });
        }

        return new MovieSearchResponse(moviesToDto);
    }
}