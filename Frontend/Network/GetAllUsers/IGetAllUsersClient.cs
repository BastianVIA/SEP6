using Frontend.Service;

namespace Frontend.Network.GetAllUsers;

public interface IGetAllUsersClient 
{
    public Task<List<Entities.User>> GetAllUsersAsync(UserSortingKey2? userSortingKey = null,
        SortingDirection3? sortingDirection = null,
        int? pageNumber = null);
}