using Backend.Database;
using Backend.Database.Transaction;
using Backend.User.Domain;
using Microsoft.EntityFrameworkCore;


namespace Backend.User.Infrastructure;

public class UserRepository : IUserRepository
{

    public  async Task<Domain.User> ReadUserFromIdAsync(string userId, DbReadOnlyTransaction tx, bool includeRatings = false, bool includeFavoriteMovies =false)
    {
        var query = tx.DataContext.Users.Where(u => u.Id == userId);
        if (includeRatings)
        {
            query = query.Include(u => u.UserRatings);
        }

        if (includeFavoriteMovies)
        {
            query = query.Include(u => u.FavoriteMovies);
        }
    
    public async Task<Domain.User> ReadUserFromIdAsync(string userId , DbReadOnlyTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.FavoriteMovies)
            .Include(u => u.UserRatings)
            .SingleAsync(user => user.Id == userId);
        return ToDomain(user);
    }

    public async Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId, DbReadOnlyTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.UserRatings)
            .SingleAsync(user => user.Id == userId);
        return ToDomain(user);
    }
    
    public async Task<Domain.User> ReadUserWithFavouriteMoviesFromIdAsync(string userId, DbReadOnlyTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == userId);
        var user = await query.SingleOrDefaultAsync();
        if (user == null)
        {
            throw new KeyNotFoundException($"Could not find user with id: {userId}");
        }

        return ToDomain(user);
    }

    public async Task CreateUserAsync(string userId , DbTransaction tx)
    public async Task CreateUserAsync(string userId, string displayName, string email , DbTransaction tx)
    {

        await tx.DataContext.Users.AddAsync(new UserDAO
        {
            Id = userId,
            DisplayName = displayName,
            Email = email,
            FavoriteMovies = new List<UserMovieDAO>(),
            UserRatings = new List<UserRatingDAO>()
        });
        await tx.DataContext.SaveChangesAsync();
    }


    public async Task Update(Domain.User domainUser , DbTransaction tx)
    {
        tx.AddDomainEvents(domainUser.ReadAllDomainEvents());
        var user = await tx.DataContext.Users
            .Include(u => u.FavoriteMovies)
            .Include(u => u.UserRatings)
            .SingleAsync(user => user.Id == domainUser.Id);
        
        
        if (domainUser.FavoriteMovies != null)
        {
            if (user.FavoriteMovies == null)
            {
                user.FavoriteMovies = new List<UserMovieDAO>();
            }

        if (user.UserRatings == null)
        {
            user.UserRatings = new List<UserRatingDAO>();
        }
        FromDomain(user.FavoriteMovies, domainUser.FavoriteMovies);
        FromDomain(user.UserRatings, domainUser.Ratings);
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
        tx.DataContext.Users.Update(user);
        await tx.DataContext.SaveChangesAsync();
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
                userRatings.Add(new UserRating(rating.MovieId, rating.NumberOfStars));
            }
        }

        return new Domain.User
        {
            Id = userDao.Id,
            DisplayName = userDao.DisplayName,
            Email = userDao.Email,
            Bio = userDao.Bio,
            FavoriteMovies = favMovies,
            Ratings = userRatings
        };
    }

    

   

    private void FromDomain(List<UserMovieDAO> userDaoMovies, List<string> movieIds)
    {
        userDaoMovies.RemoveAll(daoMovie => !movieIds.Contains(daoMovie.Id));
        foreach (var movieId in movieIds)
        {
            var movieExists = userDaoMovies.Any(daoMovie => daoMovie.Id == movieId);
            if (!movieExists)
            {
                userDaoMovies.Add(new UserMovieDAO { Id = movieId });
            }
        }
    }

    private void FromDomain(List<UserRatingDAO> userDaoRatings, List<UserRating> domainRatings)
    {
        var movieIds = domainRatings.Select(r => r.MovieId).ToList();
        userDaoRatings.RemoveAll(daoRating => !movieIds.Contains(daoRating.MovieId));
        foreach (var domainRating in domainRatings)
        {
            var ratingExists = userDaoRatings.Any(daoRating => daoRating.MovieId == domainRating.MovieId);
            if (!ratingExists)
            {
                userDaoRatings.Add(new UserRatingDAO { MovieId = domainRating.MovieId, NumberOfStars = domainRating.NumberOfStars });
            }
        }
    }
}