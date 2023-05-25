using Frontend.Entities;

namespace Frontend.Network.PersonSearch;

public interface IPersonSearchClient
{
    public Task<(int NumberOfPage, List<Person> PersonList)> SearchForPersonAsync(string name, int? pageNumber = null);
}