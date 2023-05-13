using Frontend.Entities;

namespace Frontend.Events;

public class AlertBoxHandler
{
    public enum AlertType
    {
        LoginSuccess,
        LoginFail,
        SignupSuccess,
        SignupFail
    }

    private static readonly Dictionary<AlertType, Func<string, Alert>> Alerts = new()
    {
        { AlertType.LoginSuccess , LoginSuccess},
        { AlertType.LoginFail , LoginFailed}
    };
    

    private static Alert LoginSuccess(string name)
    {
        return new Alert()
        {
            Success = true,
            Message = $"Welcome {name}!",
            Description = $"You have successfully created an account."
        };
    }

    private static Alert LoginFailed(string reason)
    {
        return new Alert
        {
            Success = false,
            Message = "Sign Up Failed!",
            Description = $"Reason: {reason}."
        };
    }
    
    public Alert? GetAlert(AlertType type, string data)
    {
        if (!Alerts.TryGetValue(type, out var alertMethod)) return null;
    
        return alertMethod.Invoke(data);
    }
}