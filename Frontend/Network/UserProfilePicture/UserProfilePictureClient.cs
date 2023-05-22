using System.Net.Http.Headers;
using Frontend.Service;

namespace Frontend.Network.UserProfilePicture;

public class UserProfilePictureClient : NSwagBaseClient, IUserProfilePictureClient
{
    public async Task UploadProfilePicture(string userToken, byte[] profilePicture)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var body = new SetUserImageRequest();
        body.ImageData = profilePicture;
        await _api.UserImagePUTAsync(body);
    }

    public async Task<byte[]> GetProfilePicture(string userId)
    {
        try
        {
            var response = await _api.UserImageGETAsync(userId);
            return response.UserImageDto.ImageData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}