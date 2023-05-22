using Backend.Database.TransactionManager;
using Backend.Enum;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.SearchUser;

public record Query
(string userName, UserSortingKey UserSortingKey, SortingDirection sortingDirection,
    int pageNumber) : IRequest<UserSearchResponse>;

public record UserSearchResponse(List<UserDto> UserDtos);

public class UserDto
{
    public string DisplayName { get; set; }
    public string Id { get; set; }
    public int RatedMovie { get; set; }

    public class QueryHandler : IRequestHandler<Query, UserSearchResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

        public QueryHandler(IUserRepository repository,
            IDatabaseTransactionFactory databaseTransactionFactory)
        {
            _repository = repository;
            _databaseTransactionFactory = databaseTransactionFactory;
        }

        public async Task<UserSearchResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
            var foundUsers = await _repository.SearchForUser(request.userName, request.UserSortingKey,
                request.sortingDirection,
                request.pageNumber, transaction);
            var userToDto = new List<UserDto>();
            foreach (var foundUser in foundUsers)
            {
                var userToAdd = new UserDto
                {
                    Id = foundUser.Id,
                    DisplayName = foundUser.DisplayName,
                    RatedMovie = foundUser.FavoriteMovies.Count
                };

                userToDto.Add(userToAdd);
            }

            return new UserSearchResponse(userToDto);
        }
    }
}