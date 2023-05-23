namespace Backend.Service;

public interface IImageService
{
    Task<Uri?> GetPathForPosterAsync(string id);
}