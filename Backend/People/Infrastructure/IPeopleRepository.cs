using Backend.Database.Transaction;

namespace Backend.People.Infrastructure;

public interface IPeopleRepository
{
    Task<Domain.Person> ReadPersonFromIdAsync(string id, DbReadOnlyTransaction tx, bool includeActed = false,
        bool includeDirected = false);

    Task<List<Domain.Person>> SearchForPersonAsync(string name, int requestPageNumber, DbReadOnlyTransaction tx);
    Task<List<Domain.Person>> FindPersonsAsync(List<string> personIds, DbReadOnlyTransaction tx);
}