using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return await Task.FromResult(new AuthenticationState(AnonymousUser));
    }
    private ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));

    private ClaimsPrincipal User
    {
        get
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "user")
            };
            var identity = new ClaimsIdentity(claims, "user");
            return new ClaimsPrincipal(identity);
        }
    }
    
    public async void SignIn() {
        var result = Task.FromResult(new AuthenticationState(User));
        NotifyAuthenticationStateChanged(result);
    }
    
    public async void SignOut() {
        var result = Task.FromResult(new AuthenticationState(AnonymousUser));
        NotifyAuthenticationStateChanged(result);
    }
}