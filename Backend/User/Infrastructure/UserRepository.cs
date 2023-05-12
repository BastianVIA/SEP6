using System.Data.Common;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Backend.User.Infrastructure;

public class UserRepository : IUserRepository
{
    private IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Domain.User> ReadUserFromIdAsync(string userId)
    {
        await using var _database = new DataContext(_configuration);
        var user = await _database.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == userId);
        return ToDomain(user);
    }

    public async Task CreateUserAsync(string userId)
    {
        await using var _database = new DataContext(_configuration);

        await _database.Users.AddAsync(new UserDAO
        {
            Id = userId,
            FavoriteMovies = new List<UserMovieDAO>()
        });
        await _database.SaveChangesAsync();
    }


    public async Task Update(Domain.User domainUser)
    {
        await using var _database = new DataContext(_configuration);
        var user = await _database.Users.Include(u => u.FavoriteMovies).SingleAsync(user => user.Id == domainUser.Id);
        if (user.FavoriteMovies == null)
        {
            user.FavoriteMovies = new List<UserMovieDAO>();
        }
        user.FavoriteMovies = FromDomain(user.FavoriteMovies, domainUser.FavoriteMovies);
        _database.Users.Update(user);
        await _database.SaveChangesAsync();
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
    
    private List<UserMovieDAO> FromDomain(List<UserMovieDAO> userDaoMovies, List<string> movieIds)
    {
        var userMovieDaos = new List<UserMovieDAO>();
        userMovieDaos.AddRange(userDaoMovies);
        foreach (var movieId in movieIds)
        { 
            var movieExists = userDaoMovies.Any(daoMovie => daoMovie.Id == movieId);
            if (!movieExists)
            {
                userMovieDaos.Add(new UserMovieDAO { Id = movieId });
            }
        }

        return userMovieDaos;
    }
}