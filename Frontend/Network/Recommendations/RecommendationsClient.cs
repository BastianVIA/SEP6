using Frontend.Entities;

namespace Frontend.Network.Recommendations;

public class RecommendationsClient : NSwagBaseClient, IRecommendationsClient
{
    public async Task<List<Movie>> GetRecommendations()
    {
        var response = await _api.RecommendationsAsync();
        List<Movie> movies = new List<Movie>();
        Rating rating = new Rating();
        foreach (var movie in response.MovieRecommendations)
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

    public RecommendationsClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}