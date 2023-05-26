using Backend.Database.Transaction;
using Backend.User.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.User.Infrastructure;

public class UserImageRepository : IUserImageRepository
{
    public async Task UploadImageAsync(string userId, byte[] image, DbTransaction tx)
    {
        var toCheckIfExists = await tx.DataContext.UserProfiles.Where(p => p.Id == userId).CountAsync();

        if (toCheckIfExists == 0)
        {
            tx.DataContext.UserProfiles.Add(new UserProfileDAO { Id = userId, ProfilePicture = image });
            return;
        }

        var profile = await tx.DataContext.UserProfiles.FirstAsync(p => p.Id == userId);
        profile.ProfilePicture = image;
        tx.DataContext.Update(profile);
    }

    public async Task<byte[]?> ReadImageForUserAsync(string userId, DbReadOnlyTransaction tx)
    {
        var profile = await tx.DataContext.UserProfiles.FirstOrDefaultAsync(p => p.Id == userId);
        if (profile == null)
        {
            return null;
        }

        return profile.ProfilePicture;
    }
}