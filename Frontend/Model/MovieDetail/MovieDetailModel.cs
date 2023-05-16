using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieDetail;

public class MovieDetailModel : IMovieDetailModel

{
    private static readonly string BASEURI = "http://localhost:5276";
    private const string DEFAULT_POSTER_URL = "/Images/NoPosterAvailable.webp"; 

    public async Task<Movie?> GetMovieDetails(string movieId, string? userToken)
    {
        HttpClient httpClient = new HttpClient();
        if (userToken != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        }
        var api = new Client(BASEURI, httpClient);
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
            IsFavorite = response.MovieDetailsDto.IsFavorite,
            PosterUrl = response.MovieDetailsDto.PathToPoster == null || string.IsNullOrWhiteSpace(response.MovieDetailsDto.PathToPoster.ToString()) ? new Uri(DEFAULT_POSTER_URL, UriKind.Relative) : response.MovieDetailsDto.PathToPoster,
            Rating = new Rating
            {
                AverageRating = response.MovieDetailsDto.Ratings?.AverageRating ?? 0,
                RatingCount = response.MovieDetailsDto.Ratings?.NumberOfVotes ?? 0
            },
            Actors = actors,
            Directors = directors,
            Resume = response.MovieDetailsDto.Resume
        };

        return movie;
    }
}