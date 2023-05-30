using Backend.Database.TransactionManager;
using Backend.Enum;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetAllUsers;

public record Query
(string userName, UserSortingKey UserSortingKey, SortingDirection sortingDirection,
    int pageNumber) : IRequest<UserSearchResponse>;

public record UserSearchResponse(List<UserDto> UserDtos);

public class UserDto
{
    public string DisplayName { get; set; }
    public string Id { get; set; }
    public int RatedMovie { get; set; }
    public byte[]? Image { get; set; }
}

    public class QueryHandler : IRequestHandler<Query, UserSearchResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IUserImageRepository _imageRepository;
        private readonly IDatabaseTransactionFactory _databaseTransactionFactory;
        

    public QueryHandler(IUserRepository repository,
        IDatabaseTransactionFactory databaseTransactionFactory, IUserImageRepository imageRepository)
    {
        _repository = repository;
        _databaseTransactionFactory = databaseTransactionFactory;
        _imageRepository = imageRepository;
    }
    
    public async Task<UserSearchResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var users = new List<Domain.User>();
        

        if (string.IsNullOrWhiteSpace(request.userName))
        {
            users = await _repository.GetAllUsersAsync(request.UserSortingKey, request.sortingDirection, request.pageNumber,transaction);
        }
        
        var userToDto = new List<UserDto>();
        foreach (var foundUser in users)
        {
            var userImage = await _imageRepository.ReadImageForUserAsync(foundUser.Id, transaction);
            var userToAdd = new UserDto
            {
                Id = foundUser.Id,
                DisplayName = foundUser.DisplayName,
            };
            if (foundUser.Ratings != null)
            {
                userToAdd.RatedMovie = foundUser.Ratings.Count;
            }

            if (userImage != null)
            {
                userToAdd.Image = userImage;
            }
            

            userToDto.Add(userToAdd);
        }

        return new UserSearchResponse(userToDto);
    }

}
