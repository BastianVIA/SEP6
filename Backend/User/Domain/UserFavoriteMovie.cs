using System.ComponentModel.DataAnnotations;

namespace Backend.User.Domain;

public class UserFavoriteMovie
{
    public string MovieId { get; set; }
    public DateTime TimeMovieWasAdded { get; set; }
    public UserFavoriteMovie(){}

    public UserFavoriteMovie(string movieId)
    {
        if (movieId == "")
        {
            throw new ValidationException(
                $"Trying to add a UserFavoriteMovie with empty movieId, this is not allowed, a UserFavoriteMovie has to have a movieId");
        }        
        MovieId = movieId;
        TimeMovieWasAdded = DateTime.Now;
    }
}