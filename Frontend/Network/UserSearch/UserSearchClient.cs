using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.UserSearch;

public class UserSearchClient : NSwagBaseClient, IUserSearchClient
{
    public async Task<List<Entities.User>> SearchForUserAsync(string displayName,UserSortingKey2? userSortingKey = null,
        SortingDirection3? sortingDirection = null, int? pageNumber = null)
    {
        var response = await _api.Search3Async(displayName,userSortingKey,sortingDirection,pageNumber);
        List<Entities.User> users = new List<Entities.User>();
        foreach (var user in response.UserDtos)
        {
            var userToAdd = new Entities.User
            {
                Id = user.Id,
                Username = user.DisplayName,
                RatedMovies = user.RatedMovie,
            };
            if (user.Image != null){
                var profilePictureString = Convert.ToBase64String(user.Image, 0, user.Image.Length);
                var imageAsString = $"data:image/jpg;base64,{profilePictureString}";
                userToAdd.ProfilePicture = imageAsString;
            }
            users.Add(userToAdd);
        
        }

        return users;
    }
    
    public async Task<List<Entities.User>> SearchForAllUsersAsync(UserSortingKey? userSortingKey = null, SortingDirection? sortingDirection = null,
        int? pageNumber = null)
    {
        var response = await _api.SearchAllAsync(userSortingKey, sortingDirection, pageNumber);

        List<Entities.User> users = new List<Entities.User>();

        foreach (var user in response.UserDtos)
        {
            var userToAdd = new Entities.User
            {
                Id = user.Id,
                Username = user.DisplayName,
                RatedMovies = user.RatedMovie,
            };

            if (user.Image != null)
            {
                var profilePictureString = Convert.ToBase64String(user.Image, 0, user.Image.Length);
                var imageAsString = $"data:image/jpg;base64,{profilePictureString}";
                userToAdd.ProfilePicture = imageAsString;
            }

            users.Add(userToAdd);
        }

        return users;
    }


    public UserSearchClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}