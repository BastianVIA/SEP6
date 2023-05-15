using Frontend.Events;

namespace Frontend.Model.Firebase;

public interface IFirebaseModel
{
    public event EventHandler<AlertEventArgs> OnNotifyAlert; 
    public string DisplayName { get; }
    public string TokenValue { get; }
    public string UID { get; }
    public Task<bool> CreateUser(string displayName, string email, string password);
    public Task<bool> Login(string email, string password);
}