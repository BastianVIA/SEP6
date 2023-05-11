using System.Text.Json;
using Frontend.Entities;
using Frontend.Service;
namespace Frontend.Model.MovieDetailModel;

public class MovieDetailModel : IMovieDetailModel

{
    private static readonly Uri BASEURI = new Uri("http://localhost:5276");
    private const string DEFAULT_POSTER_URL = "/Images/NoPosterAvailable.webp"; 

    public async Task<Movie?> GetMovieDetails(string movieId)
    {
        var api = new Client(BASEURI.ToString(), new HttpClient());

        MovieDetailsResponse? response;

        try
        {
            response = await api.MovieAsync(movieId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        if (response == null || response.MovieDetailsDto == null)
        {
            return null;
        }

        var actors = response.MovieDetailsDto.Actors?.Select(actor => new Actor
        {
            ID = actor.Id,
            Name = actor.Name,
            BirthYear = actor.BirthYear
        }).ToList();

        var directors = response.MovieDetailsDto.Directors?.Select(director => new Director
        {
            ID = director.Id,
            Name = director.Name,
            BirthYear = director.BirthYear
        }).ToList();

        var movie = new Movie
        {
            Id = response.MovieDetailsDto.Id,
            Title = response.MovieDetailsDto.Title,
            ReleaseYear = response.MovieDetailsDto.ReleaseYear,
            PosterUrl = response.MovieDetailsDto.PathToPoster == null || string.IsNullOrWhiteSpace(response.MovieDetailsDto.PathToPoster.ToString()) ? new Uri(DEFAULT_POSTER_URL, UriKind.Relative) : response.MovieDetailsDto.PathToPoster,
            Rating = new Rating
            {
                AverageRating = response.MovieDetailsDto.Ratings?.AverageRating ?? 0,
                RatingCount = response.MovieDetailsDto.Ratings?.NumberOfVotes ?? 0
            },
            Actors = actors,
            Directors = directors,
        };

        return movie;
    }

    
}