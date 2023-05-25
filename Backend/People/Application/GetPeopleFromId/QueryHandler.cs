using Backend.Database.TransactionManager;
using Backend.People.Application.Search;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using MediatR;

namespace Backend.People.Application.GetPeopleFromId;

public record Query(List<string> personIds) : IRequest<PersonResponse>;

public record PersonResponse(List<PersonDto> PersonDtos);

public class QueryHandler : IRequestHandler<Query, PersonResponse>
{
    private readonly IPeopleRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IPeopleRepository repository, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<PersonResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var requestedPeople = await _repository.FindPersonsAsync(request.personIds, transaction);
        return ToDto(requestedPeople);
    }

    private PersonResponse ToDto(List<Person> requestedPeople)
    {
        List<PersonDto> personDtos = new List<PersonDto>();
        foreach (var person in requestedPeople)
        {
            personDtos.Add(ToDto(person));
        }

        return new PersonResponse(personDtos);
    }

    private PersonDto ToDto(Person person)
    {
        return new PersonDto
        {
            BirthYear = person.BirthYear,
            ID = person.Id,
            Name = person.Name
        };
    }
}