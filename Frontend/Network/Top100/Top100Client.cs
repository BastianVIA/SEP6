using Frontend.Entities;

namespace Frontend.Network.Top100;

public class Top100Client :NSwagBaseClient, ITop100Client
{
    public async Task<List<Movie>> GetTop100(int pageNumber)
    {
        var response = await _api.TopAsync(pageNumber);
        List<Movie> movies = new List<Movie>();
        Rating rating = new Rating();
        foreach (var movie in response.TopMovies)
        {
            try
            {
                rating = new Rating { AverageRating = movie.Rating.AverageRating, RatingCount = movie.Rating.Votes };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Uri.TryCreate(movie.PathToPoster?.ToString(), UriKind.Absolute, out Uri? posterUri);
            movies.Add(new Movie
            {
                Id = movie.Id, 
                Title = movie.Title, 
                ReleaseYear = movie.ReleaseYear, 
                PosterUrl = posterUri ?? new Uri(DEFAULT_POSTER_URL, UriKind.Relative),
                Rating = rating
            });
        }
        return movies;
    }

    public Top100Client(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}