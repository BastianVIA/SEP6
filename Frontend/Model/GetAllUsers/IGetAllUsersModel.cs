using Frontend.Service;

namespace Frontend.Model.GetAllUsers;

public interface IGetAllUsersModel
{
    Task<List<Entities.User> > GetAllUsersAsync(UserSortingKey2? userSortingKey = null, SortingDirection3? sortingDirection = null, int? pageNumber = null);

}