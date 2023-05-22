using Backend.Database.Transaction;

namespace Backend.People.Infrastructure;

public interface IPeopleRepository
{
    Task<Domain.Person> ReadPersonFromId(string id, DbReadOnlyTransaction tx, bool includeActed = false,
        bool includeDirected = false);

    Task<List<Domain.Person>> SearchForPerson(string name, int requestPageNumber, DbReadOnlyTransaction tx);
    Task<List<Domain.Person>> FindPersons(List<string> personIds, DbReadOnlyTransaction tx);
}