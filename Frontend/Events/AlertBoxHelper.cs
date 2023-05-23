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
        LogoutSuccess,
        AddFavoriteMovieSuccess,
        AddFavoriteMovieFail,
        RemoveFavouriteMovieSuccess,
        RemoveFavouriteMovieFail,
        SetRatingSuccess,
        SetRatingFail,
        CreateReviewSuccess,
        CreateReviewFail,
        UploadProfileImageSuccess,
        UploadProfileImageFail
    }

    private static readonly Dictionary<AlertType, Func<string, Alert>> Alerts = new()
    {
        { AlertType.LoginSuccess , LoginSuccess},
        { AlertType.LoginFail , LoginFailed},
        { AlertType.SignupSuccess , SignupSuccess},
        { AlertType.SignupFail , SignupFailed},
        { AlertType.LogoutSuccess , LogoutSuccess},
        { AlertType.AddFavoriteMovieSuccess, AddFavouriteSuccess},
        { AlertType.AddFavoriteMovieFail , AddFavouriteFailed},
        { AlertType.RemoveFavouriteMovieSuccess , RemoveFavouriteSuccess},
        { AlertType.RemoveFavouriteMovieFail , RemoveFavouriteFailed},
        { AlertType.SetRatingSuccess , SetRatingSuccess},
        { AlertType.SetRatingFail , SetRatingFailed},
        { AlertType.CreateReviewSuccess , CreateRatingSuccess},
        { AlertType.CreateReviewFail, CreateRatingFailed},
        { AlertType.UploadProfileImageSuccess , UploadProfileImageSuccess},
        { AlertType.UploadProfileImageFail , UploadProfileImageFailed}
    };

    private static Alert RemoveFavouriteFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "Remove Favourite Error",
            Message = message
        };
    }

    private static Alert RemoveFavouriteSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "Remove Favourite Success",
            Message = message
        };
    }

    private static Alert UploadProfileImageFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "Image Upload Error",
            Message = message
        };
    }

    private static Alert UploadProfileImageSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "Image Upload Success",
            Message = message
        };
    }

    private static Alert CreateRatingFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "New Review Error",
            Message = message
        };
    }

    private static Alert CreateRatingSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "New Review Success",
            Message = message
        };
    }

    private static Alert SetRatingFailed(string message)
    {
        return new Alert
        {
            Success = false,
            Header = "New Rating Error",
            Message = message
        };
    }

    private static Alert SetRatingSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "New Rating Success",
            Message = message
        };
    }

    private static Alert AddFavouriteFailed(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "New Favourite Error",
            Message = message
        };
    }

    private static Alert AddFavouriteSuccess(string message)
    {
        return new Alert
        {
            Success = true,
            Header = "New Favourite Success",
            Message = message
        };
    }

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