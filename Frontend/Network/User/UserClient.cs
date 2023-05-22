using System.Net.Http.Headers;
using Frontend.Service;

namespace Frontend.Network.User;

public class UserClient : NSwagBaseClient, IUserClient
{
    public async Task CreateUser(string userToken, string displayName, string email)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        await _api.UserPOSTAsync(new CreateUserRequest{DisplayName = displayName, Email = email});
}

    public UserClient(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory, configuration)
    {
    }
}