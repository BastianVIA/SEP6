using Firebase.Auth;
using Firebase.Auth.Providers;
using Frontend.Entities;

namespace Frontend.Network.Firebase;

public class FirebaseClient : IFirebaseClient
{
    private string API_KEY;
    private string DOMAIN;
    private static FirebaseAuthConfig config;
    private FirebaseAuthClient client;
    
    public FirebaseClient(IConfiguration configuration)
    {
        API_KEY = configuration.GetConnectionString("FirebaseAPI");
        DOMAIN = configuration.GetConnectionString("FirebaseDomain");
        
        config = new FirebaseAuthConfig
        {
            ApiKey = API_KEY,
            AuthDomain = DOMAIN,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };
        client = new FirebaseAuthClient(config);
    }

    public async Task<FirebaseUser> CreateUser(string displayName, string email, string password)
    {
        var userCredential = await client.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
        
        var tokenValue = await userCredential.User.GetIdTokenAsync();
        return CreateFirebaseUser(userCredential, tokenValue);
    }

    public async Task<FirebaseUser> Login(string email, string password)
    {
        var userCredential = await client.SignInWithEmailAndPasswordAsync(email, password);
        var tokenValue = await userCredential.User.GetIdTokenAsync();
        Console.WriteLine($"TOKEN: {tokenValue}");
        return CreateFirebaseUser(userCredential, tokenValue);
    }

    public void SignOut()
    {
        client.SignOut();
    }

    private FirebaseUser CreateFirebaseUser(UserCredential credential, string tokenValue)
    {
        return new FirebaseUser
        {
            DisplayName = credential.User.Info.DisplayName,
            TokenValue = tokenValue,
            UID = credential.User.Uid
        };
    }
}