using Backend.Movie.Application.GetMovieInfo;
using Backend.Movie.Application.Search;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetFavoriteMovies;

public record Query(string userId) : IRequest<FavoriteMovesResponse>;

public record FavoriteMovesResponse(List<FavoriteMovieDto> movies);

public class FavoriteMovieDto
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public FavoriteMovieRatingDto? Rating { get; set; }
}

public record FavoriteMovieRatingDto(float AverageRating, int Votes);

public class QueryHandler : IRequestHandler<Query, FavoriteMovesResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;

    public QueryHandler(IUserRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<FavoriteMovesResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var userRequested = await _repository.ReadUserFromIdAsync(request.userId);
        var movieDtos = new List<FavoriteMovieDto>();
        foreach (var userFavoriteMovie in userRequested.FavoriteMovies)
        {
            var queryForMovie = new Movie.Application.GetMovieInfo.Query(userFavoriteMovie);
            var movieInfo = await _mediator.Send(queryForMovie, cancellationToken);
            movieDtos.Add(toDto(movieInfo));
        }

        return new FavoriteMovesResponse(movieDtos);
    }

    private FavoriteMovieDto toDto(MovieInfoDto movieInfoDto)
    {
        var favMovie = new FavoriteMovieDto
        {
            Id = movieInfoDto.Id,
            Title = movieInfoDto.Title,
            ReleaseYear = movieInfoDto.ReleaseYear,
            PathToPoster = movieInfoDto.PathToPoser
        };
        if (movieInfoDto.Rating != null)
        {
            favMovie.Rating = new FavoriteMovieRatingDto(movieInfoDto.Rating.AverageRating, movieInfoDto.Rating.Votes);
        }
        
        return favMovie;
    }
   
}