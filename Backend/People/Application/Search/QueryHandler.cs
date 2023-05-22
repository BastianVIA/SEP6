using Backend.Database.TransactionManager;
using Backend.People.Infrastructure;
using MediatR;

namespace Backend.People.Application.Search;

public record Query(string name, int pageNumber) : IRequest<PersonSearchResponse>;

public record PersonSearchResponse(List<PersonDto> PersonDtos);

public class PersonDto
{
    public string Name { get; set; }
    public string ID { get; set; }
    public int? BirthYear { get; set; }
}
public class QueryHandler : IRequestHandler<Query, PersonSearchResponse>
{
    private readonly IPeopleRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IPeopleRepository repository, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<PersonSearchResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var foundPersons = await _repository.SearchForPerson(request.name, request.pageNumber, transaction);
        var personsToDto = new List<PersonDto>();
        foreach (var foundPerson in foundPersons)
        {
            var personToAdd = new PersonDto
            {
                ID = foundPerson.Id,
                Name = foundPerson.Name,
                BirthYear = foundPerson.BirthYear
            };
            
            personsToDto.Add(personToAdd);
        }

        return new PersonSearchResponse(personsToDto);
    }
}