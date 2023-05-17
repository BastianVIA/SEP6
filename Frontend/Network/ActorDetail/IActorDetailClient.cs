namespace Frontend.Network.Actor;

public interface IActorDetailClient
{
    public Task<Entities.Actor> GetActorDetail(string actorId);
}