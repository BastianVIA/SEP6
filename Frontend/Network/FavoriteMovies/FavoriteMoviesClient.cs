using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.FavoriteMovies;

public class FavoriteMoviesClient : NSwagBaseClient, IFavoriteMoviesClient
{
    public async Task<IList<Movie>> GetFavoriteMovies(string UID)
    {
        FavoriteMovesResponse response;
        try
        {
            response = await _api.FavoriteGETAsync(UID);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        List<Movie> movies = new List<Movie>();
        Rating rating = new Rating();
        foreach (var movie in response.Movies)
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

    public async Task AddToFavoriteMovies(string bearerToken, string movieId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        await _api.FavoritePUTAsync(movieId);
    }
    
    public async Task DeleteFavoriteMovie(string bearerToken, string movieId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        await _api.FavoritePUTAsync(movieId);
    }

    public FavoriteMoviesClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}