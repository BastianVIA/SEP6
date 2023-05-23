namespace Backend.Service;

public class UserImageService : IUserImageService
{
    private string _imageFolder;
    public UserImageService()
    {
        var root = Directory.GetCurrentDirectory();
        _imageFolder = Path.Combine(root, "Images");
    }
    
    public async Task<byte[]?> GetImageDataAsync(string userId)
    {
        var path = Path.Combine(_imageFolder, $"{userId}.jpg");

        try
        {
            return await File.ReadAllBytesAsync(path);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task UploadImageAsync(string userId, byte[] data)
    {
        string filePath = Path.Combine(_imageFolder, $"{userId}.jpg");

        try
        {
            await File.WriteAllBytesAsync(filePath, data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}