using Frontend.Service;

namespace Frontend.Network.UserSearch;

public interface IUserSearchClient
{
    public Task<List<Entities.User>> SearchForUserAsync(string username, string sortingAlphabet, 
        SortingDirection? sortingDirection = null, int? pageNumber = null);
}