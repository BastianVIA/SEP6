using Backend.Service;

namespace Backend.Movie.Infrastructure;

public class MovieRepository : IMovieRepository
{
    private IMovieService _movieService;

    public MovieRepository(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<List<Domain.Movie>> SearchForMovie(string title)
    {
        var moviesFound = await _movieService.SearchForMovie(title);

        return ToDomain(moviesFound);
    }

    private List<Domain.Movie> ToDomain(List<ServiceMovie> serviceMovies)
    {
        var domainMovies = new List<Domain.Movie>();
        foreach (var serviceMovie in serviceMovies)
        {
            domainMovies.Add(new Domain.Movie
            {
                Title = serviceMovie.Title,
                Id = serviceMovie.Id
            });
        }
        return domainMovies;
    }
}