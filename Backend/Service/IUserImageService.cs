namespace Backend.Service;

public interface IUserImageService
{
    public Task<byte[]?> GetImageDataAsync(string userId);
    public Task UploadImageAsync(string userId, byte[] data);
}