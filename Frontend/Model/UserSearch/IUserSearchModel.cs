using Frontend.Service;

namespace Frontend.Model.UserSearch;

public interface IUserSearchModel
{
    Task<IList<Entities.User>> SearchForUserAsync(string displayName, UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null);
}