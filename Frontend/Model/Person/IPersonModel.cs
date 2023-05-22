namespace Frontend.Model.Person;

public interface IPersonModel
{
    public Task<Entities.Person> GetPersonDetails(string actorId);
}