namespace Frontend.Model.FirebaseModel;

public interface IFirebaseModel
{
    public Task CreateUser();
    public Task Login();
}