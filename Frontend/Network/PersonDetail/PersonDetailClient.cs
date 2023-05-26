using System.Net.Http.Headers;
using Frontend.Entities;

namespace Frontend.Network.PersonDetail;

public class PersonDetailClient : NSwagBaseClient, IPersonDetailClient
{
    public async Task<Person> GetPersonDetail(string personId)
    {
        var response = await _api.PersonAsync(personId);
        var actedMovies = response.ActedMovies?.Select(movie => new Movie
        {
            Id = movie.MovieId,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PosterUrl = movie.PathToPoster,
            Rating = movie.Rating != null
                ? new Rating { AverageRating = movie.Rating.AvgRating, RatingCount = movie.Rating.Votes }
                : null
        }).ToList();

        var directedMovies = response.DirectedMovies?.Select(movie => new Movie
        {
            Id = movie.MovieId,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PosterUrl = movie.PathToPoster,
            Rating = movie.Rating != null
                ? new Rating { AverageRating = movie.Rating.AvgRating, RatingCount = movie.Rating.Votes }
                : null
        }).ToList();

        var person = new Person
        {
            ID = response.Id,
            Name = response.Name,
            ActedInList = actedMovies,
            DirectedList = directedMovies,
            BirthYear = response.BirthYear,
            ImageUrl = response.PathToPic ?? new Uri(DEFUALT_PERSON_IMAGE_URL, UriKind.Relative),
            Bio = response.Bio,
            KnownFor = response.KnownFor
        };
        return person;
    }

    public PersonDetailClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory,
        configuration)
    {
    }
}