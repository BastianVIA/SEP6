using Frontend.Service;

namespace Frontend.Model.UserSearch;

public interface IUserSearchModel
{
    Task<IList<Entities.User>> SearchForUserAsync(string username, string sortingAlphabet, 
        SortingDirection? sortingDirection = null, int? pageNumber = null);
}