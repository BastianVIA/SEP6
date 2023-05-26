using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.MovieSearch;

public class MovieSearchClient : NSwagBaseClient, IMovieSearchClient
{
    public async Task<List<Movie>> SearchForMovieAsync(string title, MovieSortingKey? movieSortingKey = null,
        SortingDirection2? sortingDirection = null, int? pageNumber = null)
    {
        var response = await _api.SearchAsync(title, movieSortingKey, sortingDirection, pageNumber);
        List<Movie> movies = new List<Movie>();
        Rating rating = new Rating();
        foreach (var movie in response.MovieDtos)
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

    public MovieSearchClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
        
    }
}