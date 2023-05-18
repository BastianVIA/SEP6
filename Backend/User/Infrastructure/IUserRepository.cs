using Backend.Database.Transaction;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.User.Infrastructure;

public interface IUserRepository
{
<<<<<<<<< Temporary merge branch 1
    Task<Domain.User> ReadUserFromIdAsync(string userId);
    Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId);
    Task CreateUserAsync(string userId);
    Task Update(Domain.User user);
=========
    Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId, DbReadOnlyTransaction tx);
    Task CreateUserAsync(string userId, DbTransaction tx);
    Task Update(Domain.User user, DbTransaction tx);
}