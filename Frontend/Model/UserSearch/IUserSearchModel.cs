using Frontend.Service;

namespace Frontend.Model.UserSearch;

public interface IUserSearchModel
{
    Task<(int NumberOfPages, List<Entities.User> UserList)> SearchForUserAsync(string displayName, UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null);
}