using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetRecommendations;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.UserProfile;


public record Query(string userId) : IRequest<UserProfileResponse>;
public record UserProfileResponse(UserProfileDto userProfile);

public class UserProfileDto
{
    public List<UserRatingDto> Ratings { get; set; }
}

public class UserRatingDto
{
    public int NumberOfStars { get; set; }
}


public class QueryHandler : IRequestHandler<Query,UserProfileResponse>
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
        var transaction =   _transactionFactory.BeginReadOnlyTransaction();
        
        var userRequested = await _repository.ReadUserWithFavouriteMoviesFromIdAsync(request.userId, transaction);
        var ratingDtos = new List<UserRatingDto>();
        foreach (var rating in userRequested.Ratings)
        {
            ratingDtos.Add(new UserRatingDto{NumberOfStars = rating.NumberOfStars});
        }
        
        return new UserProfileResponse(toDto(ratingDtos));
    }
    
    
    private UserProfileDto toDto(List<UserRatingDto> userRating)
    {
        return new UserProfileDto { Ratings = userRating};
    }
}