using Frontend.Entities;

namespace Frontend.Network.Actor;

public class ActorDetailClient : IActorDetailClient
{
    public async Task<Entities.Actor> GetActorDetail(string actorId)
    {
        Console.WriteLine("in client: " + actorId);
        return MockActor();
    }

    private Entities.Actor MockActor()
    {
        var movies = new List<Movie>();
        movies.Add(new Movie()
        {
            Id = "tt0109830",
            Title = "Forest Gump",
            ReleaseYear = 1994
        });
        movies.Add(new Movie()
        {
            Id = "tt0120815",
            Title = "Saving Ryan fyren",
            ReleaseYear = 1998
        });
        return new Entities.Actor()
        {
            Name = "Katteøje",
            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id tempus eros, sed dignissim metus. Nam non sem in libero fringilla rhoncus. Donec eu dui eleifend, tristique mi at, elementum metus. Etiam gravida congue sem a porta. Sed tempor sit amet neque non pharetra. Aenean viverra lorem dui, id varius justo elementum faucibus. Aliquam tempus magna eget lacus malesuada venenatis. Sed interdum vitae purus a hendrerit.",
            ImageUrl = new Uri("https://www.themoviedb.org/t/p/w300_and_h450_bestv2/xRk889LiJsKlijIVp8KfHiZWw7X.jpg"),
            BirthYear = 1984,
            ActedInList = movies
            
        };
    }
}