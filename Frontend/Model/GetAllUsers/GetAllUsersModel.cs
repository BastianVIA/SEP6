using Frontend.Network.GetAllUsers;
using Frontend.Service;

namespace Frontend.Model.GetAllUsers;

public class GetAllUsersModel :IGetAllUsersModel
{
    private IGetAllUsersClient _client;
    public GetAllUsersModel(IGetAllUsersClient client)
    {
        _client = client;
    }

    public async Task<List<Entities.User>> GetAllUsersAsync(UserSortingKey2? userSortingKey = null, SortingDirection3? sortingDirection = null,
        int? pageNumber = null)
    {
        return await _client.GetAllUsersAsync(userSortingKey,sortingDirection, pageNumber);
    }
}