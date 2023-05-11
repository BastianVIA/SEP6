using Firebase.Auth;
using Firebase.Auth.Providers;

namespace Frontend.Model.FirebaseModel;

public class FirebaseModel : IFirebaseModel
{
    private const string API_KEY = "AIzaSyDt7-Qj0cqMjASG_Nj0CH_eNwevS5L36vQ";
    private const string DOMAIN = "sep6-a072b.firebaseapp.com";

    static FirebaseAuthConfig config = new FirebaseAuthConfig
    {
        ApiKey = API_KEY,
        AuthDomain = DOMAIN,
        Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider()
        }
    };

    FirebaseAuthClient client = new FirebaseAuthClient(config);
    
    public async Task CreateUser()
    {
        var userCredential = await client.CreateUserWithEmailAndPasswordAsync("bastianthomsen@live.dk", "123Mathias", "Bastian");
    }

    public async Task Login()
    {
        var userCredential = await client.SignInWithEmailAndPasswordAsync("bastianthomsen@live.dk", "123Mathias");
        var token = userCredential.User.GetIdTokenAsync();
        var displayName = userCredential.User.Info.DisplayName;
    }
}