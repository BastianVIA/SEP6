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


public class QueryHandler : IRequestHandler<Query, UserProfileResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;

    public QueryHandler(IUserRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public async Task<UserProfileResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var userRequested = await _repository.ReadUserWithRatingsFromIdAsync(request.userId);
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