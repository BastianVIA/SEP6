using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieSearchModel;

public class MovieSearchModel : IMovieSearchModel
{
    private const string BASEURI = "http://localhost:5276";
    public async Task<List<Movie>> SearchForMovieAsync(string title)
    {
        var api = new Client(BASEURI, new HttpClient());
        var response = await api.SearchAsync(title);
        List<Movie> movies = new List<Movie>();
        foreach (var movie in response.MovieDtos)
        {
            movies.Add(new Movie{Id = movie.Id, Title = movie.Title, ReleaseYear = movie.ReleaseYear, ImagePath = movie.PathToPoster});
        }
        return movies;
    }
}