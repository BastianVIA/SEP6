using Frontend.Entities;

namespace Frontend.Events;

public class AlertBoxHelper
{
    public enum AlertType
    {
        LoginSuccess,
        LoginFail,
        SignupSuccess,
        SignupFail,
        LogoutSuccess
    }

    private static readonly Dictionary<AlertType, Func<string, Alert>> Alerts = new()
    {
        { AlertType.LoginSuccess , LoginSuccess},
        { AlertType.LoginFail , LoginFailed},
        { AlertType.SignupSuccess , SignupSuccess},
        { AlertType.SignupFail , SignupFailed},
        { AlertType.LogoutSuccess , LogoutSuccess}
    };

    private static Alert LoginSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "Login Successful!",
            Message = message
        };
    }

    private static Alert LoginFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "Login Failed!",
            Message = message
        };
    }

    private static Alert SignupSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "Signup Success!",
            Message = message
        };
    }

    private static Alert SignupFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "Signup Failed!",
            Message = message
        };
    }
    
    private static Alert LogoutSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "Logout Successful!",
            Message = message
        };
    }

    public Alert? GetAlert(AlertType type, string message)
    {
        if (!Alerts.TryGetValue(type, out var alertMethod)) return null;
    
        return alertMethod.Invoke(message);
    }
}