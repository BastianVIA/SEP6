using Backend.Database.Transaction;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.User.Infrastructure;

public interface IUserRepository
{
    Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task<Domain.User> ReadUserWithFavouriteMoviesFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task CreateUserAsync(string userId, string displayName, string email, DbTransaction tx);
    Task Update(Domain.User user, DbTransaction tx);
}