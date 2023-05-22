using Frontend.Service;

namespace Frontend.Network.UserSearch;

public interface IUserSearchClient
{
    public Task<List<Entities.User>> SearchForUserAsync(string displayName, UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null);
}   