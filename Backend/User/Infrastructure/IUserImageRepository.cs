using Backend.Database.Transaction;

namespace Backend.User.Infrastructure;

public interface IUserImageRepository
{
    Task UploadImageAsync(string userId, byte[] image, DbTransaction tx);
    Task<byte[]?> ReadImageForUserAsync(string userId, DbReadOnlyTransaction tx);
}