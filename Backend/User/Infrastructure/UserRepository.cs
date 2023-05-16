using Backend.Database;
using Backend.Database.Transaction;
using Backend.User.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using NLog;

namespace Backend.User.Infrastructure;

public class UserRepository : IUserRepository
{
    
    public async Task<Domain.User> ReadUserFromIdAsync(string userId , DbReadOnlyTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == userId);
        return ToDomain(user);
    }

    public async Task<Domain.User> ReadUserWithRatingsFromIdAsync(string userId, DbReadOnlyTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == userId);
        return ToDomain(user);
    }

    public async Task CreateUserAsync(string userId , DbTransaction tx)
    {

        await tx.DataContext.Users.AddAsync(new UserDAO
        {
            Id = userId,
            FavoriteMovies = new List<UserMovieDAO>()
        });
        await tx.DataContext.SaveChangesAsync();
    }


    public async Task Update(Domain.User domainUser , DbTransaction tx)
    {
        var user = await tx.DataContext.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == domainUser.Id);
        if (user.FavoriteMovies == null)
        {
            user.FavoriteMovies = new List<UserMovieDAO>();
        }
        FromDomain(user.FavoriteMovies, domainUser.FavoriteMovies);
        tx.DataContext.Users.Update(user);
        await tx.DataContext.SaveChangesAsync();
    }

    private Domain.User ToDomain(UserDAO userDao)
    {
        var favMovies = new List<string>();
        if (userDao.FavoriteMovies != null)
        {
            foreach (var userDaoFavoriteMovie in userDao.FavoriteMovies)
            {
                favMovies.Add(userDaoFavoriteMovie.Id);
            }
        }

        return new Domain.User
        {
            Id = userDao.Id,
            FavoriteMovies = favMovies
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
    
}