using Frontend.Service;

namespace Frontend.Model.UserSearch;

public interface IUserSearchModel
{
    Task<List<Entities.User> > SearchForUserAllUsersAsync(UserSortingKey? userSortingKey = null, SortingDirection? sortingDirection = null, int? pageNumber = null);

    Task<List<Entities.User> > SearchForUserAsync(string displayName, UserSortingKey2? userSortingKey = null, SortingDirection3? sortingDirection = null, int? pageNumber = null);
}