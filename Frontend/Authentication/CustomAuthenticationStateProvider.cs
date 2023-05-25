using System.Security.Claims;
using Frontend.Model.Firebase;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly IFirebaseModel _firebaseModel;
    private ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));

    public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage, IFirebaseModel firebaseModel)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _firebaseModel = firebaseModel;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _protectedLocalStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(AnonymousUser));
            }

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Email, userSession.Email),
                new Claim(ClaimTypes.UserData, userSession.Password),
                new Claim(ClaimTypes.Role, userSession.Role),
            }, "CustomAuth"));
            await _firebaseModel.Login(userSession.Email, userSession.Password);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(AnonymousUser));
        }
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await _protectedLocalStorage.SetAsync("UserSession", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.Email),
                new Claim(ClaimTypes.UserData, userSession.Password),
                new Claim(ClaimTypes.Role, userSession.Role)
            }));
        }
        else
        {
            await _protectedLocalStorage.DeleteAsync("UserSession");
            claimsPrincipal = AnonymousUser;
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}