using System.Data.Common;
using NLog;

namespace Backend.User.Domain;

public class User
{
    public string Id { get; set; }
    public List<string> FavoriteMovies { get; set; }

    public User()
    {
        FavoriteMovies = new List<string>();
    }
    public bool HasAlreadyFavoritedMovie(string movieId)
    {
        foreach (var favoriteMovie in FavoriteMovies)
        {
            if (favoriteMovie == movieId)
            {
                return true;
            }
        }
        return false;
    }
}