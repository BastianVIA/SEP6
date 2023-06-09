using Backend.Database.Transaction;
using Backend.Enum;
using Backend.User.Domain;
using Backend.User.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.User.Infrastructure;

public class UserRepository : IUserRepository

{
    private int NrOfResultsEachPage;

    public UserRepository(IConfiguration configuration)
    {
        NrOfResultsEachPage = configuration.GetSection("QueryConstants").GetValue<int>("UsersPerPage");
    }

    public async Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx, bool includeRatings = false, bool includeFavoriteMovies =false, bool includeReviews = false)
    {
        var query = tx.DataContext.Users.Where(u => u.Id == userId);
        if (includeRatings)
        {
            query = query.Include(u => u.UserRatings);
        }

        if (includeFavoriteMovies)
        {
            query = query.Include(u => u.FavoriteMovies.OrderByDescending(m => m.TimeMovieWasAdded ));
        }

        if (includeReviews)
        {
            query = query.Include(u => u.UserReviews);
        }

        var user = await query.SingleOrDefaultAsync();
        if (user == null)
        {
            throw new KeyNotFoundException($"Could not find user with id: {userId}");
        }

        return ToDomain(user);
    }
        
    public async Task<List<Domain.User>> SearchForUserAsync(string displayName, UserSortingKey userSortingKey, SortingDirection sortingDirection, int requestPageNumber,
        DbReadOnlyTransaction tx)
    {
        var query = tx.DataContext.Users.Include(u=> u.FavoriteMovies)
            .Include(u => u.UserRatings )
            .Where(u => EF.Functions.Like(u.DisplayName, $"%{displayName}%"));

        switch (userSortingKey)
        {
            case UserSortingKey.DisplayName:
                query = SearchForUserOrderByUsernameAsync(query, sortingDirection);
                break;
            case UserSortingKey.MoviesVoted:
                query = SearchForUserOrderByVotedMoviesAsync(query, sortingDirection);
                break; ;
            default:
                throw new KeyNotFoundException($"{userSortingKey} not a valid user sorting key ");
        }
        
        List<UserDAO> foundUsers = await query
            .Skip(NrOfResultsEachPage * (requestPageNumber - 1))
            .Take(NrOfResultsEachPage)
            .ToListAsync();

        return ToDomain(foundUsers);
    }

    public async Task CreateUserAsync(Domain.User user, DbTransaction tx)
    {
        tx.AddDomainEvents(user.ReadAllDomainEvents());
        await tx.DataContext.Users.AddAsync(new UserDAO
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email
        });
    }

 
    
    public async Task UpdateAsync(Domain.User domainUser , DbTransaction tx)
    {
        tx.AddDomainEvents(domainUser.ReadAllDomainEvents());
        var user = await tx.DataContext.Users
            .Include(u => u.UserReviews)
            .Include(u => u.FavoriteMovies)
            .Include(u => u.UserRatings)
            .FirstAsync(user => user.Id == domainUser.Id);
        
        
        if (domainUser.FavoriteMovies != null)
        {
            if (user.FavoriteMovies == null)
            {
                user.FavoriteMovies = new List<UserFavoriteMovieDAO>();
            }

            FromDomain(user.FavoriteMovies, domainUser.FavoriteMovies);
        }

        if (domainUser.Ratings != null)
        {
            if (user.UserRatings == null)
            {
                user.UserRatings = new List<UserRatingDAO>();
            }
            FromDomain(user.UserRatings, domainUser.Ratings);
        }

        if (domainUser.Reviews != null)
        {
            if (user.UserReviews == null)
            {
                user.UserReviews = new List<UserReviewDAO>();
            }
            FromDomain(user.UserReviews, domainUser.Reviews);
        }

        tx.DataContext.Users.Update(user);
    }
    
    public async Task<int> NumberOfResultsForSearch(string requestDisplayName, DbReadOnlyTransaction tx)
    {
        var query = tx.DataContext.Users
            .Where(u => EF.Functions.Like(u.DisplayName, $"%{requestDisplayName}%"));
        return await query.CountAsync();
    }
    
    public async Task<List<Domain.User>> GetAllUsersAsync(UserSortingKey userSortingKey, SortingDirection sortingDirection, int requestPageNumber, DbReadOnlyTransaction tx)
    {
        IOrderedQueryable<UserDAO> query; 

        switch (userSortingKey)
        {
            case UserSortingKey.DisplayName:
                query = SearchForUserOrderByUsernameAsync(tx.DataContext.Users.Include(u => u.FavoriteMovies).Include(u => u.UserRatings), sortingDirection);
                break;
            case UserSortingKey.MoviesVoted:
                query = SearchForUserOrderByVotedMoviesAsync(tx.DataContext.Users.Include(u => u.FavoriteMovies).Include(u => u.UserRatings), sortingDirection);
                break;
            default:
                throw new KeyNotFoundException($"{userSortingKey} not a valid user sorting key ");
        }
        
        List<UserDAO> foundUsers = await query
            .Skip(NrOfResultsEachPage * (requestPageNumber - 1))
            .Take(NrOfResultsEachPage)
            .ToListAsync();

        return ToDomain(foundUsers);
        
    }

    
    private IOrderedQueryable<UserDAO> SearchForUserOrderByUsernameAsync(IQueryable<UserDAO> query,
        SortingDirection sortingDirection)
    {
        switch (sortingDirection)
        {
            case SortingDirection.DESC:
                return query.OrderByDescending(user => user.DisplayName == null).ThenByDescending(user => user.DisplayName);
            case SortingDirection.ASC:
                return query.OrderBy(user => user.DisplayName == null).ThenBy(user => user.DisplayName);
            default:
                throw new KeyNotFoundException($"{sortingDirection} not a valid order direction ");
        }
    }
    
    
    private IOrderedQueryable<UserDAO> SearchForUserOrderByVotedMoviesAsync(IQueryable<UserDAO> query,
        SortingDirection sortingDirection)
    {
        switch (sortingDirection)
        {
            case SortingDirection.DESC:
                return query.OrderByDescending(user => user.FavoriteMovies.Count);
            case SortingDirection.ASC:
                return query.OrderBy(user => user.FavoriteMovies.Count);
            default:
                throw new KeyNotFoundException($"{sortingDirection} not a valid order direction ");
        }
    }

    private List<Domain.User> ToDomain(List<UserDAO> userDaos)
    {
        return userDaos.Select(userDao => ToDomain(userDao)).ToList();
    }
    private Domain.User ToDomain(UserDAO userDao)
    {
        List<string>? favMovies = null;
        if (userDao.FavoriteMovies != null)
        {
            favMovies = new List<string>();
            foreach (var userDaoFavoriteMovie in userDao.FavoriteMovies)
            {
                favMovies.Add(userDaoFavoriteMovie.Id);
            }
        }

        List<UserRating>?  userRatings = null;
        if (userDao.UserRatings != null)
        {
            userRatings = new List<UserRating>();

            foreach (var rating in userDao.UserRatings)
            {
                userRatings.Add(new UserRating { MovieId = rating.MovieId, NumberOfStars = rating.NumberOfStars });
            }
        }

        List<UserReview>? userReviews = null;
        if (userDao.UserReviews != null)
        {
            userReviews = new List<UserReview>();

            foreach (var review in userDao.UserReviews)
            {
                userReviews.Add(new UserReview{MovieId = review.MovieId, ReviewBody = review.Body});
            }
        }

        List<UserFavoriteMovie>? userFavoriteMovies = null;
        if (userDao.FavoriteMovies != null)
        {
            userFavoriteMovies = new List<UserFavoriteMovie>();
            foreach (var favoriteMovie in userDao.FavoriteMovies)
            {
                userFavoriteMovies.Add(new UserFavoriteMovie{MovieId = favoriteMovie.Id, TimeMovieWasAdded = favoriteMovie.TimeMovieWasAdded});
            }
        }
        return new Domain.User
        {
            Id = userDao.Id,
            DisplayName = userDao.DisplayName,
            Email = userDao.Email,
            Bio = userDao.Bio,
            FavoriteMovies = userFavoriteMovies,
            Ratings = userRatings,
            Reviews = userReviews
        };
    }

    
    private void FromDomain(List<UserReviewDAO> userDaoReviews, List<UserReview> domainReviews)
    {
        var movieIds = domainReviews.Select(r => r.MovieId).ToList();
        userDaoReviews.RemoveAll(daoMovie => !movieIds.Contains(daoMovie.MovieId));
        foreach (var review in domainReviews)
        {
            var existingReview = userDaoReviews.FirstOrDefault(daoReview => daoReview.MovieId == review.MovieId);

            if (existingReview == null)
            {
                userDaoReviews.Add(new UserReviewDAO{MovieId = review.MovieId, Body = review.ReviewBody});
            }
            else
            {
                existingReview.Body = review.ReviewBody;
            }
            
        }
    }
   

    private void FromDomain(List<UserFavoriteMovieDAO> userDaoMovies, List<UserFavoriteMovie> favoriteMovies)
    {
        var movieIds = favoriteMovies.Select(f => f.MovieId);
        userDaoMovies.RemoveAll(daoMovie => !movieIds.Contains(daoMovie.Id));
        foreach (var movie in favoriteMovies)
        {
            var movieExists = userDaoMovies.Any(daoMovie => daoMovie.Id == movie.MovieId);
            if (!movieExists)
            {
                userDaoMovies.Add(new UserFavoriteMovieDAO { Id = movie.MovieId, TimeMovieWasAdded = movie.TimeMovieWasAdded});
            }
        }
    }

    private void FromDomain(List<UserRatingDAO> userDaoRatings, List<UserRating> domainRatings)
    {
        var movieIds = domainRatings.Select(r => r.MovieId).ToList();
        userDaoRatings.RemoveAll(daoRating => !movieIds.Contains(daoRating.MovieId));

        foreach (var domainRating in domainRatings)
        {
            var existingRating = userDaoRatings.FirstOrDefault(daoRating => daoRating.MovieId == domainRating.MovieId);

            if (existingRating == null)
            {
                userDaoRatings.Add(new UserRatingDAO { MovieId = domainRating.MovieId, NumberOfStars = domainRating.NumberOfStars });
            }
            else
            {
                existingRating.NumberOfStars = domainRating.NumberOfStars;
            }
        }
    }

}