namespace Backend.Movie.Infrastructure;

public interface IMovieRepository
{
    Task<List<Domain.Movie>> SearchForMovie(string title);

}