using Frontend.Entities;
using Frontend.Events;

namespace Frontend.Model.Firebase;

public interface IFirebaseModel
{
    public event EventHandler<AlertEventArgs> OnNotifyAlert;
    public FirebaseUser CurrentUser { get; }
    public Task<bool> CreateUser(string displayName, string email, string password);
    public Task<bool> Login(string email, string password);
    public void Logout();
    
}