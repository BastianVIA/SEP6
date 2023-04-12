namespace Backend.Service;

public class TMDBService : IMovieService
{
    private const string APIKEY = "api_key=2fc86a533431c3559a968522a4955362";

    private const string BASEURI =
        "https://api.themoviedb.org/3";


    public async Task<List<ServiceMovie>> SearchForMovie(string keyWordSearch)
    {
        var searchMovieURL = BASEURI +
                             "/search/movie?" +
                             APIKEY +
                             $"&include_adult=true" +
                             $"&query={keyWordSearch}";
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(searchMovieURL);
        MovieResponse? responseAsMovieResponse =  await response.Content.ReadFromJsonAsync<MovieResponse>();
        var listOfMovies = new List<ServiceMovie>();
        foreach (var result in responseAsMovieResponse.Results)
        {
            listOfMovies.Add(ToServiceMovie(result));
        }

        return listOfMovies;
    }

    private ServiceMovie ToServiceMovie(Movie movie)
    {
        return new ServiceMovie
        {
            Title = movie.Title,
            Id = movie.Id
        };
    }
    public class MovieResponse
    {
        public Movie[] Results { get; set; }
    }

    public class Movie
    {
        public string PosterPath { get; set; }
        public bool Adult { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; }
        public int Id { get; set; }
        public string OriginalTitle { get; set; }
        public string OriginalLanguage { get; set; }
        public string Title { get; set; }
        public string BackdropPath { get; set; }
        public decimal Popularity { get; set; }
        public int VoteCount { get; set; }
        public bool Video { get; set; }
        public decimal VoteAverage { get; set; }
    }

}