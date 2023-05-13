using Frontend.Entities;

namespace Frontend.Events;

public class AlertBoxHelper
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
        { AlertType.LoginFail , LoginFailed},
        { AlertType.SignupSuccess , SignupSuccess},
        { AlertType.SignupFail , SignupFailed}
    };
    

    private static Alert LoginSuccess(string name)
    {
        return new Alert()
        {
            Success = true,
            Message = $"Welcome {name}!",
            Description = $"You have successfully logged in."
        };
    }

    private static Alert LoginFailed(string reason)
    {
        return new Alert
        {
            Success = false,
            Message = "Login Failed!",
            Description = $"Reason: {reason}."
        };
    }

    private static Alert SignupSuccess(string text)
    {
        return new Alert
        {
            Success = true,
            Message = "Signup Success!",
            Description = $"You have successfully created an account."
        };
    }

    private static Alert SignupFailed(string reason)
    {
        return new Alert
        {
            Success = false,
            Message = "Signup Failed!",
            Description = $"Reason: {reason}"
        };
    }

    public Alert? GetAlert(AlertType type, string data)
    {
        if (!Alerts.TryGetValue(type, out var alertMethod)) return null;
    
        return alertMethod.Invoke(data);
    }
}