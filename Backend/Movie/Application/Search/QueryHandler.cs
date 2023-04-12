using Backend.Movie.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.Search;

public class Query : IRequest<QueryResponse>
{
    public string Title { get; set; }
}
public class QueryResponse
{
    public List<MovieDto> Movies { get; set; }
}

public class MovieDto
{
    public string Title { get; set; }
    public int Id { get; set; }
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
                Title = foundMovie.Title,
                Id = foundMovie.Id
            });
        }

       return new QueryResponse
       {
           Movies = moviesToDto
       };
    }
    
    
}