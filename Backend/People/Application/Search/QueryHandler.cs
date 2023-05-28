using Backend.Database.TransactionManager;
using Backend.People.Infrastructure;
using Backend.Service;
using MediatR;

namespace Backend.People.Application.Search;

public record Query(string name, int pageNumber) : IRequest<PersonSearchResponse>;

public record PersonSearchResponse(List<PersonDto> PersonDtos);

public class PersonDto
{
    public string Name { get; set; }
    public string ID { get; set; }
    public int? BirthYear { get; set; }
    public Uri? PathToPic { get; set; }
}

public class QueryHandler : IRequestHandler<Query, PersonSearchResponse>
{
    private readonly IPeopleRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;
    private readonly IPersonService _personService;

    public QueryHandler(IPeopleRepository repository, IDatabaseTransactionFactory databaseTransactionFactory,
        IPersonService personService)
    {
        _repository = repository;
        _databaseTransactionFactory = databaseTransactionFactory;
        _personService = personService;
    }

    public async Task<PersonSearchResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var persons = await _repository.SearchForPersonAsync(request.name, request.pageNumber, transaction);

        var personTasks = persons.Select(async p =>
        {
            var personDto = await _personService.GetPersonAsync(p.ImdbId);
            return new PersonDto
            {
                ID = p.Id,
                Name = p.Name,
                BirthYear = p.BirthYear,
                PathToPic = personDto?.PathToPersonPic
            };
        });
        var personDtos = await Task.WhenAll(personTasks);
        return new PersonSearchResponse(personDtos.ToList());
    }
}