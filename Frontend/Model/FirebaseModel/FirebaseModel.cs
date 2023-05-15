﻿using Firebase.Auth;
using Firebase.Auth.Providers;
using Frontend.Entities;
using Frontend.Events;

namespace Frontend.Model.FirebaseModel;

public class FirebaseModel : IFirebaseModel
{
    private string API_KEY;
    private string DOMAIN;
    public string TokenValue { get; private set; }
    public string DisplayName { get; set; }

    public event EventHandler<AlertEventArgs>? OnNotifyAlert; 
    private static FirebaseAuthConfig config;
    private FirebaseAuthClient client;
    private AlertBoxHelper _alertBoxHelper;

    public FirebaseModel(IConfiguration configuration)
    {
        API_KEY = configuration.GetConnectionString("FirebaseAPI");
        DOMAIN = configuration.GetConnectionString("FirebaseDomain");
        
        config = new FirebaseAuthConfig
        {
            ApiKey = API_KEY,
            AuthDomain = DOMAIN,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };

        client = new FirebaseAuthClient(config);
        _alertBoxHelper = new AlertBoxHelper();
    }

    public async Task<bool> CreateUser(string displayName, string email, string password)
    {
        try
        {
            var userCredential = await client.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            TokenValue = await userCredential.User.GetIdTokenAsync();
            
            return true;
        }
        catch (FirebaseAuthException e)
        {
            //EmailExists
            //InvalidEmailAddress
            Console.WriteLine(e);
            var reason = e.Reason;
            
            return false;
        }
    }
    public async Task<bool> Login(string email, string password)
    {
        try
        {
            var userCredential = await client.SignInWithEmailAndPasswordAsync(email, password);
            TokenValue = await userCredential.User.GetIdTokenAsync();
            DisplayName = userCredential.User.Info.DisplayName;
            FireAlertEvent(AlertBoxHelper.AlertType.LoginSuccess, DisplayName);
            return true;
        }
        catch (FirebaseAuthException e)
        {
            Console.WriteLine(e);
            var reason = e.Reason.ToString();
            //WrongPassword
            //UnknownEmailAddress
            FireAlertEvent(AlertBoxHelper.AlertType.LoginFail, reason);
            return false;
        }
    }

    private void FireAlertEvent(AlertBoxHelper.AlertType type, string data)
    {
        OnNotifyAlert?.Invoke(this,new AlertEventArgs
        {
            Type = type,
            Data = data
        });
    }

}