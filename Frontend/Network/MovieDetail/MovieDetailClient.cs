using System.Net.Http.Headers;
using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Network.MovieDetail;

public class MovieDetailClient : NSwagBaseClient, IMovieDetailClient
{
    
    
    public async Task<Movie?> GetMovieDetails(string movieId, string? userToken)
    {
        if (userToken != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        }
        var response = await _api.MovieAsync(movieId);

        var actors = response.MovieDetailsDto.Actors?.Select(actor => new Entities.Person
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
            UserRating = response.MovieDetailsDto.UserRating,
            PosterUrl = response.MovieDetailsDto.PathToPoster == null || string.IsNullOrWhiteSpace(response.MovieDetailsDto.PathToPoster.ToString()) ? new Uri(DEFAULT_POSTER_URL, UriKind.Relative) : response.MovieDetailsDto.PathToPoster,
            MovieTrailer = response.MovieDetailsDto.MovieTrailer,
            Rating = new Rating
            {
                AverageRating = response.MovieDetailsDto.Ratings?.AverageRating ?? 0,
                RatingCount = response.MovieDetailsDto.Ratings?.NumberOfVotes ?? 0
            },
            Actors = actors,
            Directors = directors,
            Resume = response.MovieDetailsDto.Resume,
            IsFavorite = response.MovieDetailsDto.IsFavorite
        };

        return movie;
    }

    public async Task SetMovieRating(string? userToken, string movieId, int? rating = null)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        await _api.RateMovieAsync(new SetRatingRequest { MovieId = movieId, Rating = rating });
        
    }
    
}