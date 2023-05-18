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
    public Uri? PathToPoster { get; set; }

    public DetailsRatingDto? Ratings { get; set; }
    public List<DetailsPersonsDto>? Actors { get; set; }
    public List<DetailsPersonsDto>? Directors { get; set; }

    public string? Resume { get; set; }
}

public record DetailsRatingDto(float AverageRating, int NumberOfVotes);

public record DetailsPersonsDto(string Id, string Name, int? BirthYear);

public class QueryHandler : IRequestHandler<Query, MovieDetailsResponse>
{
    private IMovieRepository _repository;
    private readonly IImageService _imageService;
    private readonly IResumeService _resumeService;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IMovieRepository repository, IImageService imageService, IResumeService resumeService,
        IMediator mediator, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _imageService = imageService;
        _resumeService = resumeService;
        _mediator = mediator;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<MovieDetailsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var movie = await _repository.ReadMovieFromId(request.Id, transaction);
        var pathForPoster = _imageService.GetPathForPoster(request.Id);
        var resume = _resumeService.GetResume(request.Id);
        var isFavorite = false;
        if (request.userId != null)
        {
            isFavorite =
                await _mediator.Send(new User.Application.GetIfMovieIsFavorite.Query(request.userId, request.Id));
        }

        var actors = await _mediator.Send(new People.Application.GetPeopleFromId.Query(movie.Actors));
        var directors = await _mediator.Send(new People.Application.GetPeopleFromId.Query(movie.Directors));

        return new MovieDetailsResponse(ToDto(movie, await pathForPoster, await resume, isFavorite, actors, directors));
    }

    private MovieDetailsDto ToDto(Domain.Movie movie, Uri? pathToPoser, string? resume, bool isFavorite, PersonResponse actors, PersonResponse directors)
    {
        var dtoMovie = new MovieDetailsDto
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PathToPoster = pathToPoser,
            Actors = ToPersonDto(actors),
            Directors = ToPersonDto(directors),
            Resume = resume,
            IsFavorite = isFavorite
        };
        if (movie.Rating != null)
        {
            dtoMovie.Ratings = new DetailsRatingDto(movie.Rating.AverageRating, movie.Rating.Votes);
        }


        return dtoMovie;
    }

    private List<DetailsPersonsDto>? ToPersonDto(PersonResponse persons)
    {
        if (persons.PersonDtos == null || persons.PersonDtos.Count == 0)
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