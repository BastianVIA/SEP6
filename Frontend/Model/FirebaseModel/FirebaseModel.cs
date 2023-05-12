using System.Reflection.Metadata.Ecma335;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace Frontend.Model.FirebaseModel;

public class FirebaseModel : IFirebaseModel
{
    private string API_KEY;
    private string DOMAIN;
    public string TokenValue { get; private set; }
    public string DisplayName { get; set; }
    public event Action<string> OnLogin;
    
    private static FirebaseAuthConfig config;
    private FirebaseAuthClient client;

    public FirebaseModel(IConfiguration configuration)
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

    public async Task<bool> CreateUser(string displayName, string email, string password)
    {
        try
        {
            var userCredential = await client.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            TokenValue = await userCredential.User.GetIdTokenAsync();
            return true;
        }
        catch (Exception e)
        {
            //EmailExists
            //InvalidEmailAddress
            Console.WriteLine(e);
            return false;
        }
    }
    public async Task<bool> Login(string email, string password)
    {
        try
        {
            var userCredential = await client.SignInWithEmailAndPasswordAsync(email, password);
            TokenValue = await userCredential.User.GetIdTokenAsync();
            DisplayName = userCredential.User.Info.DisplayName;
            NotifyLogin();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //WrongPassword
            //UnknownEmailAddress
            return false;
        }
    }

    private void NotifyLogin()
    {
        OnLogin.Invoke(DisplayName);
    }
}