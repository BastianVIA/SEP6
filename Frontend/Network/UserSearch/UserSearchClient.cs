using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.UserSearch;

public class UserSearchClient:IUserSearchClient
{
    public async Task<List<Entities.User>> SearchForUserAsync(string username, string sortingAlphabet,
        SortingDirection? sortingDirection = null,
        int? pageNumber = null)
    {
        // var response = await _api.SearchAsync(username,sortingAlphabet, sortingDirection, pageNumber);
        // List<Entities.User> users = new List<Entities.User>();
        // Rating rating = new Rating();
        // foreach (var user in response.userDto)
        // {
        //     try
        //     {
        //         rating = new Rating { AverageRating = user.Rating.AverageRating, RatingCount = user.Rating.Votes };
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        //     Uri.TryCreate(user.PathToProfilePicture?.ToString(), UriKind.Absolute, out Uri? pictureUri);
        //     users.Add(new Entities.User
        //     {
        //         Id = user.Id, 
        //         Username = user.Username, 
        //         Email = user.Email, 
        //         ProfilePicture = pictureUri.ToString(),
        //         Rating = rating
        //     });
        // }
        // return users;
        //
        // }
        return null;
    }
    
}