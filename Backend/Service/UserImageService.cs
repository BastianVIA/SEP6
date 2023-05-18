namespace Backend.Service;

public class UserImageService : IUserImageService
{
    private string _imageFolder;
    public UserImageService()
    {
        var root = Directory.GetCurrentDirectory();
        _imageFolder = Path.Combine(root, "Images");
    }
    
    public async Task<byte[]>? GetImageData(string userId)
    {
        byte[] data;
        var path = Path.Combine(_imageFolder, $"{userId}.jpg");

        try
        {
            data = await File.ReadAllBytesAsync(path);
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

        return data;
    }
}