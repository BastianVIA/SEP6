namespace Backend.User.Infrastructure;

public interface IUserRepository
{
    Task<Domain.User> ReadUserFromIdAsync(string userId);
    Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId);
    Task CreateUserAsync(string userId);
    Task Update(Domain.User user);
}