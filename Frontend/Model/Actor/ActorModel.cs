using Frontend.Network.Actor;

namespace Frontend.Model.Actor;

public class ActorModel : IActorModel
{
    private IActorDetailClient _client;

    public ActorModel()
    {
        _client = new ActorDetailClient();
    }


    public async Task<Entities.Actor> GetActorDetails(string actorId)
    {
        return await _client.GetActorDetail(actorId);
    }
}