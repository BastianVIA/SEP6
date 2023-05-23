using Backend.Database.TransactionManager;
using Backend.Movie.Domain;
using Backend.Movie.Infrastructure;
using Backend.People.Application.GetPeopleFromId;
using Backend.Service;
using MediatR;

namespace Backend.Movie.Application.GetDetails;

public record Query(string Id, string? userId) : IRequest<MovieDetailsResponse>;

public record MovieDetailsResponse(MovieDetailsDto MovieDetailsDto);

public class MovieDetailsDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsFavorite { get; set; }
    public int? UserRating { get; set; }
    public Uri? PathToPoster { get; set; }
    
    public string? MovieTrailer { get; set; }

    public DetailsRatingDto? Ratings { get; set; }
    public List<DetailsPersonsDto>? Actors { get; set; }
    public List<DetailsPersonsDto>? Directors { get; set; }

    public string? Resume { get; set; }
}

public record DetailsRatingDto(double AverageRating, int NumberOfVotes);

public record DetailsPersonsDto(string Id, string Name, int? BirthYear);

public class QueryHandler : IRequestHandler<Query, MovieDetailsResponse>
{
    private IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IResumeService _resumeService;
    private readonly ITrailerService _trailerService;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IMovieRepository repository, IImageService imageService, IResumeService resumeService, ITrailerService trailerService,
        IMediator mediator, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _resumeService = resumeService;
        _trailerService = trailerService;
        _mediator = mediator;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<MovieDetailsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var movie = await _repository.ReadMovieFromId(request.Id, transaction,includeRatings:true,includeActors:true,includeDirectors:true);
        var pathForPoster = _imageService.GetPathForPoster(request.Id);
        var trailerPath = await _trailerService.GetMovieTrailer(request.Id);
        var resume = _resumeService.GetResume(request.Id);
        var isFavorite = false;
        int? userRating = null;
        if (request.userId != null)
        {
            var result =
                await _mediator.Send(new User.Application.GetUserInfoForMovie.Query(request.userId, request.Id));
            isFavorite = result.IsFavorite;
            userRating = result.NumberOfStars;
        }

        PersonResponse? actors = null;
        PersonResponse? directors = null;
        if (movie.Actors != null)
        {
            actors = await _mediator.Send(new People.Application.GetPeopleFromId.Query(movie.Actors));
        }

        if (movie.Directors != null)
        {
            directors = await _mediator.Send(new People.Application.GetPeopleFromId.Query(movie.Directors));
        }

        return new MovieDetailsResponse(ToDto(movie, await pathForPoster, trailerPath, await resume, isFavorite, actors, directors, userRating));
    }

    private MovieDetailsDto ToDto(Domain.Movie movie, Uri? pathToPoser, string? trailerPath, string? resume, bool isFavorite, PersonResponse? actors, PersonResponse? directors, int? userRating)
    {
        var dtoMovie = new MovieDetailsDto
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PathToPoster = pathToPoser,
            MovieTrailer = trailerPath,
            Actors = ToPersonDto(actors),
            Directors = ToPersonDto(directors),
            Resume = resume,
            IsFavorite = isFavorite,
            UserRating = userRating
        };
        if (movie.Rating != null)
        {
            dtoMovie.Ratings = new DetailsRatingDto(movie.Rating.AverageRating, movie.Rating.Votes);
        }


        return dtoMovie;
    }

    private List<DetailsPersonsDto>? ToPersonDto(PersonResponse? persons)
    {
        if (persons == null || !persons.PersonDtos.Any())
        {
            return null;
        }

        var listToReturn = new List<DetailsPersonsDto>();
        foreach (var person in persons.PersonDtos)
        {
            listToReturn.Add(new DetailsPersonsDto(person.ID, person.Name, person.BirthYear));
        }

        return listToReturn;
    }
}