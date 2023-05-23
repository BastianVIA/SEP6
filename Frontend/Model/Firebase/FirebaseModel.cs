using Firebase.Auth;
using Frontend.Entities;
using Frontend.Events;
using Frontend.Network.Firebase;

namespace Frontend.Model.Firebase;

public class FirebaseModel : IFirebaseModel
{
    private IFirebaseClient _client;
    public FirebaseUser? CurrentUser { get; private set; }

    public event EventHandler<AlertEventArgs>? OnNotifyAlert;
   // private AlertBoxHelper _alertBoxHelper;

    public FirebaseModel(IFirebaseClient client)
    {
        _client = client;
    }

    public async Task<bool> CreateUser(string displayName, string email, string password)
    {
        try
        {
            CurrentUser = await _client.CreateUser(displayName, email, password);
            FireAlertEvent(AlertBoxHelper.AlertType.SignupSuccess,
                $"You have successfully created an account.");
            return true;
        }
        catch (FirebaseAuthException e)
        {
            var reason = e.Reason.ToString();
            FireAlertEvent(AlertBoxHelper.AlertType.SignupFail, 
                $"Error creating account. Reason: {reason}");
            return false;
        }
    }
    public async Task<bool> Login(string email, string password)
    {
        try
        {
            CurrentUser = await _client.Login(email, password);
            FireAlertEvent(AlertBoxHelper.AlertType.LoginSuccess,
                $"Successfully logged in.");
            return true;
        }
        catch (FirebaseAuthException e)
        {
            var reason = e.Reason.ToString();
            FireAlertEvent(AlertBoxHelper.AlertType.LoginFail,
                $"There was an error logging in. Reason: {reason}");
            return false;
        }
    }

    public void Logout()
    {
        _client.SignOut();
        FireAlertEvent(AlertBoxHelper.AlertType.LogoutSuccess,
            "Successfully signed out.");
    }

    private void FireAlertEvent(AlertBoxHelper.AlertType type, string message)
    {
        OnNotifyAlert?.Invoke(this,new AlertEventArgs
        {
            Type = type,
            Message = message
        });
    }

}