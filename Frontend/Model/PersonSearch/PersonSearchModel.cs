using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.PersonSearch;

namespace Frontend.Model.PersonSearch;

public class PersonSearchModel : NSwagBaseClient, IPersonSearchModel
{
    private IPersonSearchClient _client;
    
    public PersonSearchModel()
    {
        _client = new PersonSearchClient();
    }
    
    public async Task<List<Entities.Person>> SearchForPersonAsync(string name, int? pageNumber = null)
    {
        return await _client.SearchForPersonAsync(name, pageNumber);
    }
}