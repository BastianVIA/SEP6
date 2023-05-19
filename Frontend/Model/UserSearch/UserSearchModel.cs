using Frontend.Network.UserSearch;
using Frontend.Service;

namespace Frontend.Model.UserSearch;

public class UserSearchModel:IUserSearchModel
{
 
    private IUserSearchClient _client;
    public UserSearchModel(IUserSearchClient client)
    {
        _client = client;
    }
    
    public async Task<IList<Entities.User>> SearchForUserAsync(string username, int? pageNumber = null)
    {
        return await _client.SearchForUserAsync(username, pageNumber);
    }
}