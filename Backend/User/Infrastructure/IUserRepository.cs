using Backend.Database.Transaction;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.User.Infrastructure;

public interface IUserRepository
{
    Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task CreateUserAsync(string userId, DbTransaction tx);
    Task Update(Domain.User user, DbTransaction tx);
}