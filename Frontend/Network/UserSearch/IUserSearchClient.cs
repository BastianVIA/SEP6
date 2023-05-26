using Frontend.Service;

namespace Frontend.Network.UserSearch;

public interface IUserSearchClient
{
    public Task<List<Entities.User>> SearchForAllUsersAsync(UserSortingKey? userSortingKey = null,
        SortingDirection? sortingDirection = null,
        int? pageNumber = null);
    public Task<List<Entities.User>> SearchForUserAsync(string displayName, UserSortingKey2? userSortingKey = null, SortingDirection3? sortingDirection = null, int? pageNumber = null);
}   