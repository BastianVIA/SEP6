using Backend.Database.TransactionManager;
using Backend.Movie.GetTopMoviesForPerson;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.People.Application.GetDetails;

public record Query(string PersonId) : IRequest<GetPersonDetailsResponse>;

public record GetPersonDetailsResponse(string Name, List<PersonDetailsMovieDto> ActedMovies,
    List<PersonDetailsMovieDto> DirectedMovies, int? BirthYear, string? KnownFor = null, Uri? PathToPic = null,
    string? Bio = null);

public class PersonDetailsMovieDto
{
    public string MovieId { get; set; }
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
}

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
            await _repository.ReadPersonFromId(request.PersonId, transaction, includeActed: true,
                includeDirected: true);
        var personDetails = _personService.GetPersonAsync(person.ImdbId);
        var movies = _mediator.Send(new Movie.GetTopMoviesForPerson.Query(request.PersonId));


        return toDto(person, await personDetails, await movies);
    }

    public GetPersonDetailsResponse toDto(Person person, PersonDto? personDetails,
        GetTopMoviesForPersonResponse personsMovies)
    {
        var actedMovies = toDto(personsMovies.ActedMovies);
        var directedMovies = toDto(personsMovies.DirectedMovies);
        
        if (personDetails == null)
        {
            return new GetPersonDetailsResponse(person.Name,actedMovies,directedMovies , person.BirthYear);
        }
        
        return new GetPersonDetailsResponse(person.Name, actedMovies, directedMovies, person.BirthYear,
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
            personMovies.Add(new PersonDetailsMovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear
            });
        }

        return personMovies;
    }
}