using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace Backend.Service;

public class TMDBImageService : IImageService
{
    private const string Apikey = "2fc86a533431c3559a968522a4955362";
    private const string DefaultImageSize = "original";

    private readonly TMDbClient _client;

    public TMDBImageService()
    {
        _client = new TMDbClient(Apikey);
        var tmdbConfig = new TMDbConfig
        {
            Images = new ConfigImageTypes
            {
                PosterSizes = new List<string>() { DefaultImageSize },
                BaseUrl = "https://image.tmdb.org/t/p/"
            }
        };
        _client.SetConfig(tmdbConfig);
    }

    public async Task<Uri?> GetPathForPoster(int id)
    {
        var movie = await _client.GetMovieAsync(id);
        if (movie == null)
        {
            Console.WriteLine($"Could not find poster for movie with id: {id}");
            return null;
        }

        return _client.GetImageUrl(DefaultImageSize, movie.PosterPath);
    }
}
