namespace Frontend.Network.UserProfilePicture;

public interface IUserProfilePictureClient
{
    public Task UploadProfilePicture(string userToken, Byte[] profilePicture);
}