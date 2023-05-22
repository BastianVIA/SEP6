using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetInfoFromMovies;
using Backend.Movie.Application.GetRecommendations;
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

public class UserRatingDto
{
    public int NumberOfStars { get; set; }
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
        var moviesInfoResponse =await _mediator.Send(new Movie.Application.GetInfoFromMovies.Query(userRequested.FavoriteMovies));
        var favoriteAvg = GetFavoritesAverage(moviesInfoResponse);
        var ratingDataPoints = GetRatingDataPoints(userRequested);
        return new UserProfileResponse(toDto(userRequested, ratingDataPoints, favoriteAvg));
    }
    

    private UserProfileDto toDto(Domain.User user, (int,int)[] ratingDataPoints, double favoriteAvg)
    {
        return new UserProfileDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Bio = user.Bio,
            FavoriteMovies = user.FavoriteMovies,
            RatingsDataPoints = ratingDataPoints,
            AverageOfUserRatings = user.AverageOfUserRatings,
            AverageOfFavoriteMovies = favoriteAvg
        };
    }

    private double GetFavoritesAverage(MoviesInfoResponse movieInfo)
    {
        var count = 0.0d;
        foreach (var movie in movieInfo.MovieInfoDtos)
        {
            count += movie.Rating.AverageRating;
        }

        return count / movieInfo.MovieInfoDtos.Count;
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


    private List<UserRatingDto> GetRatingDtos(Domain.User user)
    {
        var list = new List<UserRatingDto>();
        foreach (var rating in user.Ratings)
        {
            list.Add(new UserRatingDto{NumberOfStars = rating.NumberOfStars});
        }

        return list;
    }
}