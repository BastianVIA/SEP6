using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.PersonSearch;

namespace Frontend.Model.PersonSearch;

public class PersonSearchModel : IPersonSearchModel
{
    private IPersonSearchClient _client;
    
    public PersonSearchModel(IPersonSearchClient client)
    {
        _client = client;
    }
    
    public async Task<List<Entities.Person>> SearchForPersonAsync(string name, int? pageNumber = null)
    {
        return await _client.SearchForPersonAsync(name, pageNumber);
    }
}