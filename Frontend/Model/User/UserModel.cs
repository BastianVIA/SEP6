using System.Net.Http.Headers;
using Frontend.Network;
using Frontend.Network.User;
using Frontend.Service;

namespace Frontend.Model.User;

public class UserModel : NSwagBaseClient, IUserModel
{
    private IUserClient _client;

    public UserModel(IUserClient client)
    {
        _client = client;
    }

    public async Task CreateUser(string userToken)
    {
        await _client.CreateUser(userToken);
    }
}