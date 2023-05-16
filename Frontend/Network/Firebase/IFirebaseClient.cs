using Frontend.Entities;

namespace Frontend.Network.Firebase;

public interface IFirebaseClient
{
    public Task<FirebaseUser> CreateUser(string displayName, string email, string password);
    public Task<FirebaseUser> Login(string email, string password);

    public void SignOut();

}