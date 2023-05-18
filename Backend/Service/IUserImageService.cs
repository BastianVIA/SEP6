namespace Backend.Service;

public interface IUserImageService
{
    public Task<byte[]>? GetImageData(string userId);
}