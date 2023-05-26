using Frontend.Network;
using Frontend.Network.UserSearch;
using Frontend.Service;
namespace Frontend.Model.UserSearch;

public class UserSearchModel: IUserSearchModel
{
 
    private IUserSearchClient _client;
    public UserSearchModel(IUserSearchClient client)
    {
        _client = client;
    }

    public async Task<List<Entities.User>> SearchForUserAllUsersAsync(UserSortingKey? userSortingKey = null, SortingDirection? sortingDirection = null,
        int? pageNumber = null)
    {
        return await _client.SearchForAllUsersAsync(userSortingKey,sortingDirection, pageNumber);
    }

    public async Task<List<Entities.User>> SearchForUserAsync(string displayName,UserSortingKey2? userSortingKey = null, SortingDirection3? sortingDirection = null, int? pageNumber = null)
    {
        return await _client.SearchForUserAsync(displayName,userSortingKey,sortingDirection, pageNumber);
    }
    
}