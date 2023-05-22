using Backend.User.Application.GetUserImage;

namespace Frontend.Network.UserProfilePicture;

public interface IUserProfilePictureClient
{
    public Task UploadProfilePicture(string userToken, Byte[] profilePicture);
    public Task<byte[]> GetProfilePicture(string userId);
}