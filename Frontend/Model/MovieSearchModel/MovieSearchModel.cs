using Frontend.Entities;

namespace Frontend.Model.MovieSearchModel;

public class MovieSearchModel : IMovieSearchModel
{
    public async Task<IList<Movie>> SearchForMovieAsync(string title)
    {
        //return await client.SearchForMovie(title);
        IList<Movie> movies = new List<Movie>();
        movies.Add(new Movie{Id = 3, Title = "Titanic"});
        movies.Add(new Movie{Id = 4, Title = "Fight Club"});
        return movies;
    }
}