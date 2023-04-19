using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.Search;

public record Query(string Title) : IRequest<QueryResponse>;

public record QueryResponse(List<MovieDto> movieDtos);

public class MovieDto
{
    public string Title { get; set; }
    public int Id { get; set; }
    public int ReleaseYear { get; set; }

}

public class QueryHandler : IRequestHandler<Query, QueryResponse>
{
    private IMovieRepository _repository;

    public QueryHandler(IMovieRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<QueryResponse> Handle(Query request, CancellationToken cancellationToken)
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

        return new QueryResponse(moviesToDto);
    }
}