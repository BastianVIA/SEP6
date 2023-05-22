using Frontend.Network.PersonDetail;

namespace Frontend.Model.Person;

public class PersonModel : IPersonModel
{
    private IPersonDetailClient _client;

    public PersonModel()
    {
        _client = new PersonDetailClient();
    }


    public async Task<Entities.Person> GetPersonDetails(string actorId)
    {
        return await _client.GetPersonDetail(actorId);
    }
}