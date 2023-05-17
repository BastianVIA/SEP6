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
    public List<UserRatingDto> Ratings { get; set; }
    
    public double AverageOfUserRatings { get; set; }
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
        
        var userRequested = await _repository.ReadUserFromIdAsync(request.userId, transaction);
        var ratingDtos = GetRatingDtos(userRequested);
        userRequested.SetRatingAvg();
        
        return new UserProfileResponse(toDto(userRequested, ratingDtos));
    }
    

    private UserProfileDto toDto(Domain.User user, List<UserRatingDto> userRating)
    {
        return new UserProfileDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Bio = user.Bio,
            FavoriteMovies = user.FavoriteMovies,
            Ratings = userRating,
            AverageOfUserRatings = user.AverageOfUserRatings
        };
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