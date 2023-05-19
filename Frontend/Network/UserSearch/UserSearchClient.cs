using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.UserSearch;

public class UserSearchClient : NSwagBaseClient, IUserSearchClient
{
    public async Task<List<Entities.User>> SearchForUserAsync(string displayName,UserSortingKey? userSortingKey = null,
        SortingDirection2? sortingDirection = null, int? pageNumber = null)
    {
        var response = await _api.Search3Async(displayName,userSortingKey,sortingDirection,pageNumber);
        List<Entities.User> users = new List<Entities.User>();
        Rating rating = new Rating();
        foreach (var user in response.UserDtos)
        {
            users.Add(new Entities.User
            {
                Id = user.Id,
                Username = user.DisplayName,
                RatedMovies = user.RatedMovie,
            });
        }

        return users;
    }
}