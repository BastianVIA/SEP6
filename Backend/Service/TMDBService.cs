using System.Text.Json.Serialization;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Backend.Service;

public class TMDBService : IImageService, IResumeService, IPersonService, ITrailerService
{
    private const string DefaultImageSize = "w500";
    private string _apiKey;

    private readonly TMDbClient _client;
    private const string baseUrlForProfilePic = "https://image.tmdb.org/t/p/w500/";

    public TMDBService(IConfiguration configuration)
    {
        _apiKey = configuration.GetConnectionString("TMDBApiKey") ?? throw new InvalidOperationException("missing TMDB Key");
        
        _client = new TMDbClient(_apiKey);
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

    public async Task<string?> GetMovieTrailer(string movieId)
    {
        var movie = await _client.GetMovieAsync(movieId, MovieMethods.Videos);
        var movies = movie.Videos.Results;
        return FindTrailer(movies);
    }

    private string? FindTrailer(List<Video> movies)
    {
        foreach (var movie in movies)
        {
            if (movie.Type.Equals("Trailer") && movie.Site.Equals("YouTube"))
            {
                var key = movie.Key;
                return $"https://www.youtube.com/embed/{key}";
            }
        }
        return null;
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

  
    public async Task<PersonServiceDto?> GetPersonAsync(string id)
    {
        var tmdbId = await GetIdOfPersonAsync(id);
        if (tmdbId == null)
        {
            return null;
        }
        var person = await _client.GetPersonAsync(tmdbId.Value);
        if (person == null)
        {
            return null;
        }
        return toDto(person);
    }

    private PersonServiceDto toDto(Person person)
    {
        var pathToProfilePic = _client.GetImageUrl(DefaultImageSize, person.ProfilePath);
        return new PersonServiceDto
        {
            KnownFor = person.KnownForDepartment,
            Bio = person.Biography,
            PathToProfilePic = pathToProfilePic
        };
    }

    private async Task<int?> GetIdOfPersonAsync(string id)
    {
        string url = $"https://api.themoviedb.org/3/find/{id}?api_key={_apiKey}&external_source=imdb_id";

        using var client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        var root = await JsonSerializer.DeserializeAsync<TmdbResponse>(await response.Content.ReadAsStreamAsync());
        if (root == null || !root.PersonResults.Any())
        {
            return null;
        }
        int personId = root.PersonResults[0].Id;
        return personId;
    }
}
public class TmdbResponse
{
    [JsonPropertyName("person_results")]
    public List<TMDBPerson> PersonResults { get; set; }
}

public class TMDBPerson
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}