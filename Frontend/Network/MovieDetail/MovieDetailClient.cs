using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.MovieDetail;

public class MovieDetailClient : NSwagBaseClient, IMovieDetailClient
{
    public async Task<Movie> GetMovieDetails(string movieId, string? userToken)
    {
        if (userToken != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        }

        MovieDetailsResponse? response;
        try
        {
            response = await _api.MovieAsync(movieId);
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

        var actors = response.MovieDetailsDto.Actors?.Select(actor => new Entities.Actor
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
            IsFavorite = response.MovieDetailsDto.IsFavorite,
            Resume = response.MovieDetailsDto.Resume
        };

        return movie;
    }
}