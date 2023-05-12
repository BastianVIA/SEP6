namespace Frontend.Model.FirebaseModel;

public interface IFirebaseModel
{
    public event Action<string> OnLogin; 
    public string DisplayName { get; }
    public Task<bool> CreateUser(string displayName, string email, string password);
    public Task<bool> Login(string email, string password);
}