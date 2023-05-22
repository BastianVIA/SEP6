namespace Frontend.Model.Actor;

public interface IActorModel
{
    public Task<Entities.Actor> GetActorDetails(string actorId);
}