using Backend.Database.TransactionManager;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.People.Application.GetDetails;

public record Query(string PersonId) : IRequest<GetPersonDetailsResponse>;

public record GetPersonDetailsResponse(string Name, int? BirthYear, string? KnownFor = null, Uri? PathToPic = null,
    string? Bio = null);

public class QueryHandler : IRequestHandler<Query, GetPersonDetailsResponse>
{
    private readonly IPeopleRepository _repository;
    private readonly IPersonService _personService;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(IPersonService personService, IDatabaseTransactionFactory transactionFactory,
        IPeopleRepository repository)
    {
        _personService = personService;
        _transactionFactory = transactionFactory;
        _repository = repository;
    }

    public async Task<GetPersonDetailsResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var person =
            await _repository.ReadPersonFromId(request.PersonId, transaction, includeActed: true,
                includeDirected: true);
        var personDetails = _personService.GetPersonAsync(person.ImdbId);


        return toDto(person, await personDetails);
    }

    public GetPersonDetailsResponse toDto(Person person, PersonDto? personDetails)
    {
        if (personDetails == null)
        {
            return new GetPersonDetailsResponse(person.Name, person.BirthYear);
        }

        return new GetPersonDetailsResponse(person.Name, person.BirthYear, personDetails.KnownFor,
            personDetails.PathToProfilePic, personDetails.Bio);
    }
}