using Frontend.Service;

namespace Frontend.Model.UserSearch;

public interface IUserSearchModel
{
    Task<IList<Entities.User>> SearchForUserAsync(string username, int? pageNumber = null);
}