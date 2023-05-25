using Backend.Database.Transaction;
using Backend.People.Domain;

namespace Backend.People.Infrastructure;

public interface IPeopleRepository
{
    Task<Person> ReadPersonFromIdAsync(string id, DbReadOnlyTransaction tx, bool includeActed = false,
        bool includeDirected = false);

    Task<(List<Person> People, int NumberOfPages )> SearchForPersonAsync(string name, int requestPageNumber, DbReadOnlyTransaction tx);
    Task<List<Person>> FindPersonsAsync(List<string> personIds, DbReadOnlyTransaction tx);
    Task<int> NumberOfResultsForSearch(string requestName, DbReadOnlyTransaction tx);
}