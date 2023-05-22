namespace Frontend.Network.PersonDetail;

public interface IPersonDetailClient
{
    public Task<Entities.Person> GetPersonDetail(string actorId);
}