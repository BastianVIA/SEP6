using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieDetailModel;

public class MovieDetailModel: IMovieDetailModel
{
    private const string BASEURI = "http://localhost:5276";
    public async Task<Movie> GetMovieDetails(string movieId)
    {
        var api = new Client(BASEURI, new HttpClient());
        var response = await api.MovieAsync(movieId);
        List<Actor> actors = new List<Actor>();
        try
        {
            foreach (var actor in response.MovieDetailsDto.Actors)
            {
                actors.Add(new Actor{ID = actor.Id, Name = actor.Name, BirthYear = actor.BirthYear});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        List<Director> directors = new List<Director>();
        try
        {
            foreach (var director in response.MovieDetailsDto.Directors)
            {
                directors.Add(new Director{ID = director.Id, Name = director.Name, BirthYear = director.BirthYear});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Movie movie = new Movie
        {
            Id = response.MovieDetailsDto.Id,
            Title = response.MovieDetailsDto.Title,
            ReleaseYear = response.MovieDetailsDto.ReleaseYear,
            PosterUrl = response.MovieDetailsDto.PathToPoster,
            Rating = new Rating{AverageRating = response.MovieDetailsDto.Ratings.AverageRating, RatingCount = response.MovieDetailsDto.Ratings.NumberOfVotes},
            Actors = actors,
            Directors = directors,
        };

        return movie;
    }
}