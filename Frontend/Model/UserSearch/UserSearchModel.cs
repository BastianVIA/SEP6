using Frontend.Network.UserSearch;
using Frontend.Service;
namespace Frontend.Model.UserSearch;

public class UserSearchModel:IUserSearchModel
{
 
    private IUserSearchClient _client;
    public UserSearchModel()
    {
        _client = new UserSearchClient();
    }
    
    public async Task<IList<Entities.User>> SearchForUserAsync(string displayName,UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null)
    {
        return await _client.SearchForUserAsync(displayName,userSortingKey,sortingDirection, pageNumber);
    }
    
}