using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace Backend.Service;

public class TMDBService : IImageService, IResumeService
{
    private const string DefaultImageSize = "original";

    private readonly TMDbClient _client;

    public TMDBService(IConfiguration configuration)
    {
        
        _client = new TMDbClient(configuration.GetConnectionString("TMDBApiKey"));
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

    public async Task<Uri?> GetPathForPoster(string id)
    {
        var movie = await _client.GetMovieAsync(id);
        if (movie == null)
        {
            Console.WriteLine($"Could not find poster for movie with id: {id}");
            return null;
        }

        return _client.GetImageUrl(DefaultImageSize, movie.PosterPath);
    }

    public async Task<string?> GetResume(string id)
    {
        var movie = await _client.GetMovieAsync(id);
        if (movie == null)
        {
            Console.WriteLine($"Could not find resume for movie with id: {id}");
            return null;
        }

        return movie.Overview;
    }
}
