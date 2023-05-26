using Backend.Database.Transaction;
using Backend.Enum;

namespace Backend.User.Infrastructure;

public interface IUserRepository
{
    
    Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx, bool includeRatings = false, bool includeFavoriteMovies =false, bool includeReviews = false);
    Task<List<Domain.User>> SearchForUserAsync(string displayName, UserSortingKey userSortingKey,
        SortingDirection sortingDirection, int requestPageNumber, DbReadOnlyTransaction tx);
    Task CreateUserAsync(Domain.User user, DbTransaction tx);
    Task UpdateAsync(Domain.User user, DbTransaction tx);
    Task<int> NumberOfResultsForSearch(string requestDisplayName, DbReadOnlyTransaction tx);
    Task<List<Domain.User>> GetAllUsersAsync(DbReadOnlyTransaction tx);

}