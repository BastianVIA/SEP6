using System.Net.Http.Headers;

namespace Frontend.Network.User;

public class UserClient : NSwagBaseClient, IUserClient
{
    public async Task CreateUser(string userToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        await _api.UserPOSTAsync();
    }
}