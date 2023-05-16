using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Frontend.Entities;
using Frontend.Pages;
using Frontend.Service;

namespace Frontend.Network.UserProfiles;

public class UserProfileClient : NSwagBaseClient, IUserProfileClient
{
    public async Task<Entities.User> GetUserProfile(string userId, string? userToken)
    {
        //     if (userToken != null)
        //     {
        //         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        //     }
        //
        //     UserProfileResponse? response;
        //     try
        //     {
        //         response = await _api.UserProfileAsync(userId);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         return null;
        //     }
        //
        //     if (response == null || response.UserProfileDto == null)
        //     {
        //         return null;
        //     }
        //
        //     var user = new Entities.User()
        //     {
        //         Id = response.UserProfileDto.Id,
        //         Username = response.UserProfileDto.Username,
        //         Email = response.UserProfileDto.Email, 
        //         Bio = response.UserProfileDto.Bio,
        //         ProfilePicture =  response.UserProfileDto.ProfilePicture
        //     };
        //
        //     return user;
        // }
        return null;
    }
}