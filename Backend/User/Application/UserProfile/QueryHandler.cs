using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetMovieInfo;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.UserProfile;


public record Query(string userId) : IRequest<UserProfileResponse>;
public record UserProfileResponse(UserProfileDto userProfile);

public class UserProfileDto
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    
    public List<string> FavoriteMovies { get; set; }
    public (int, int)[] RatingsDataPoints { get; set; }
    public double AverageOfUserRatings { get; set; }
    public double AverageOfFavoriteMovies { get; set; }
}

public class QueryHandler : IRequestHandler<Query, UserProfileResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(IUserRepository repository, IMediator mediator, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _mediator = mediator;
        _transactionFactory = transactionFactory;
    }
    
    public async Task<UserProfileResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction =  _transactionFactory.BeginReadOnlyTransaction();
        
        var userRequested = await _repository.ReadUserFromIdAsync(request.userId, transaction, includeRatings: true, includeFavoriteMovies: true);
        userRequested.SetRatingAvg();
        var moviesInfoResponse =await GetMovieInfos(userRequested.FavoriteMovies);
        var favoriteAvg = GetFavoritesAverage(moviesInfoResponse);
        var ratingDataPoints = GetRatingDataPoints(userRequested);
        return new UserProfileResponse(toDto(userRequested, ratingDataPoints, favoriteAvg));
    }
    

    private UserProfileDto toDto(Domain.User user, (int,int)[] ratingDataPoints, double favoriteAvg)
    {
        List<string> favMovieIds = new List<string>();
        if (user.FavoriteMovies != null)
        {
            favMovieIds = user.FavoriteMovies.Select(m => m.MovieId).ToList();
        }
        return new UserProfileDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Bio = user.Bio,
            FavoriteMovies = favMovieIds,
            RatingsDataPoints = ratingDataPoints,
            AverageOfUserRatings = user.AverageOfUserRatings,
            AverageOfFavoriteMovies = favoriteAvg
        };
    }

    private double GetFavoritesAverage(List<GetMovieInfoResponse> movieInfos)
    {
        var count = 0.0d;
        var numberOfFavoritesWithRating = 0;
        foreach (var movie in movieInfos)
        {
            if (movie.Rating != null)
            {
                count += movie.Rating.AverageRating;
                numberOfFavoritesWithRating++;
            }
        }

        return count / numberOfFavoritesWithRating;
    }

    private (int,int)[] GetRatingDataPoints(Domain.User user)
    {
        var dataPoints = new[] {(1,0),(2,0),(3,0),(4,0),(5,0),(6,0),(7,0),(8,0),(9,0),(10,0)};
        foreach (var rating in user.Ratings)
        {
            var tupleIndex = Array.FindIndex(dataPoints, tuple => tuple.Item1 == rating.NumberOfStars);
            dataPoints[tupleIndex].Item2++;
        }

        return dataPoints;
    }

    private async Task<List<GetMovieInfoResponse>> GetMovieInfos(List<UserFavoriteMovie>? favoriteMovies)
    {
        var favoriteMovieInfo = new List<GetMovieInfoResponse>();
        if (favoriteMovies == null)
        {
            return favoriteMovieInfo;
        }

        foreach (var favoriteMovie in favoriteMovies)
        {
            var movieInfo = _mediator.Send(new Movie.Application.GetMovieInfo.Query(favoriteMovie.MovieId));
            favoriteMovieInfo.Add(await movieInfo);
        }

        return favoriteMovieInfo;
    }
}