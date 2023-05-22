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

    private static Alert LoginSuccess(string ignored)
    {
        return new Alert()
        {
            Success = true,
            Message = $"Login Successful!",
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

    private static Alert SignupSuccess(string ignored)
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
    
    private static Alert LogoutSuccess(string arg)
    {
        return new Alert()
        {
            Success = true,
            Message = $"Logout Successful!",
            Description = $"You have successfully logged out."
        };
    }

    public Alert? GetAlert(AlertType type, string data)
    {
        if (!Alerts.TryGetValue(type, out var alertMethod)) return null;
    
        return alertMethod.Invoke(data);
    }
}