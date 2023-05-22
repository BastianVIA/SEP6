using System.Net.Http.Headers;
using Frontend.Entities;

namespace Frontend.Network.PersonDetail;

public class PersonDetailClient : NSwagBaseClient, IPersonDetailClient
{
  
    public async Task<Person> GetPersonDetail(string personId)
    {
        var response = await _api.DetailsAsync(personId);
        var actedMovies = response.ActedMovies?.Select(movie => new Movie
        {
            Id = movie.MovieId,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PosterUrl = movie.PathToPoster
        }).ToList();
        
        var directedMovies = response.DirectedMovies?.Select(movie => new Movie
        {
            Id = movie.MovieId,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PosterUrl = movie.PathToPoster
        }).ToList();

        var person = new Person
        {
            ID = response.Id,
            Name = response.Name,
            ActedInList = actedMovies,
            DirectedList = directedMovies,
            BirthYear = response.BirthYear,
            ImageUrl = response.PathToPic,
            Bio = response.Bio,
            KnownFor = response.KnownFor

        };
        return person;
    }
}