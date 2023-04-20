namespace Backend.Service;

public interface IImageService
{
    Task<Uri?> GetPathForPoster(int id);
}