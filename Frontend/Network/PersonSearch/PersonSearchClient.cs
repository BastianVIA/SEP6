using Frontend.Entities;

namespace Frontend.Network.PersonSearch;

public class PersonSearchClient : NSwagBaseClient, IPersonSearchClient
{
    public async Task<List<Person>> SearchForPersonAsync(string name, int? pageNumber = null)
    {
        var response = await _api.Search2Async(name, pageNumber);
        List<Person> persons = new List<Person>();
        foreach (var person in response.PersonDtos)
        {
            persons.Add(new Person
            {
                ID = person.Id,
                Name = person.Name,
                BirthYear = person.BirthYear
            });
        }

        return persons;
    }

    public PersonSearchClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}