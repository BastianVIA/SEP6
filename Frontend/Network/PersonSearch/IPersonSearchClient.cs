using Frontend.Entities;

namespace Frontend.Network.PersonSearch;

public interface IPersonSearchClient
{
    public Task<List<Person>> SearchForPersonAsync(string name, int? pageNumber = null);
}