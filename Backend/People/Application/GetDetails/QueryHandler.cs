using System.Diagnostics;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetTopMoviesForPerson;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.People.Application.GetDetails;

public record Query(string PersonId) : IRequest<GetPersonDetailsResponse>;

public record GetPersonDetailsResponse(string Id, string Name, List<PersonDetailsMovieDto> ActedMovies,
    List<PersonDetailsMovieDto> DirectedMovies, int? BirthYear, string? KnownFor = null, Uri? PathToPic = null,
    string? Bio = null);

public class PersonDetailsMovieDto
{
    public string MovieId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public Uri? PathToPoster { get; set; }
    public PersonMovieRating? Rating { get; set; }
}

public record PersonMovieRating(double AvgRating, int Votes);

public class QueryHandler : IRequestHandler<Query, GetPersonDetailsResponse>
{
    private readonly IPeopleRepository _repository;
    private readonly IPersonService _personService;
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IMediator _mediator;

    public QueryHandler(IPersonService personService, IDatabaseTransactionFactory transactionFactory,
        IPeopleRepository repository, IMediator mediator)
    {
        _personService = personService;
        _transactionFactory = transactionFactory;
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<GetPersonDetailsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var person =
            await _repository.ReadPersonFromIdAsync(request.PersonId, transaction);
        var personDetails = _personService.GetPersonAsync(person.ImdbId);
        var movies = _mediator.Send(new Movie.Application.GetTopMoviesForPerson.Query(request.PersonId));


        return toDto(person, await personDetails, await movies);
    }

    public GetPersonDetailsResponse toDto(Person person, PersonServiceDto? personDetails,
        GetTopMoviesForPersonResponse personsMovies)
    {
        var actedMovies = toDto(personsMovies.ActedMovies);
        var directedMovies = toDto(personsMovies.DirectedMovies);

        if (personDetails == null)
        {
            return new GetPersonDetailsResponse(person.Id, person.Name, actedMovies, directedMovies, person.BirthYear);
        }

        return new GetPersonDetailsResponse(person.Id, person.Name, actedMovies, directedMovies, person.BirthYear,
            personDetails.KnownFor, personDetails.PathToProfilePic, personDetails.Bio);
    }

    private List<PersonDetailsMovieDto> toDto(List<TopMoviesDto>? movies)
    {
        var personMovies = new List<PersonDetailsMovieDto>();
        if (movies == null)
        {
            return personMovies;
        }

        foreach (var m in movies)
        {
            var topMovie = new PersonDetailsMovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                PathToPoster = m.PathToPoster
            };
            if (m.Rating != null)
            {
                topMovie.Rating = new PersonMovieRating(m.Rating.AvgRating, m.Rating.Votes);
            }
            personMovies.Add(topMovie);
        }

        return personMovies;
    }
}