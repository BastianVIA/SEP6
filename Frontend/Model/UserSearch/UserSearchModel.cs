using Frontend.Network;
using Frontend.Network.UserSearch;
using Frontend.Service;
namespace Frontend.Model.UserSearch;

public class UserSearchModel:NSwagBaseClient, IUserSearchModel
{
 
    private IUserSearchClient _client;
    public UserSearchModel(IUserSearchClient client,IConfiguration configuration,IHttpClientFactory clientFactory):base(clientFactory,configuration)
    {
        _client = client;
    }
    
    public async Task<IList<Entities.User>> SearchForUserAsync(string displayName,UserSortingKey? userSortingKey = null, SortingDirection2? sortingDirection = null, int? pageNumber = null)
    {
        return await _client.SearchForUserAsync(displayName,userSortingKey,sortingDirection, pageNumber);
    }
    
}