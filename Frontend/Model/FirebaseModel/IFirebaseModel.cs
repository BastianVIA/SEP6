namespace Frontend.Model.FirebaseModel;

public interface IFirebaseModel
{
    public Task<bool> CreateUser(string displayName, string email, string password);
    public Task Login();
}