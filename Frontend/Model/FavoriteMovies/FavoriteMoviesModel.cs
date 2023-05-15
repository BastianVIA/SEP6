using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;


namespace Frontend.Model.FavoriteMovies;

public class FavoriteMoviesModel : IFavoriteMoviesModel
{
    private const string BASEURI = "http://localhost:5276";
    private const string DEFAULT_POSTER_URL = "/Images/NoPosterAvailable.webp";

    public async Task<IList<Movie>> GetFavoriteMovies(string userToken)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var api = new Client(BASEURI, httpClient);
        FavoriteMovesResponse response;
        try
        {
            response = await api.FavoriteGETAsync(userToken);
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
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        var api = new Client(BASEURI, httpClient);
        
        await api.FavoritePUTAsync(movieId);
    }
}