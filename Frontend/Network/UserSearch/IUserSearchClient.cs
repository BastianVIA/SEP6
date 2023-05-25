using Frontend.Service;

namespace Frontend.Network.UserSearch;

public interface IUserSearchClient
{
    public Task<(int NumberOfPages, List<Entities.User> UserList)> SearchForUserAsync(string displayName, UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null);
}   