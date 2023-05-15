using System.Net.Http.Headers;
using Frontend.Service;

namespace Frontend.Model.User;

public class UserModel : IUserModel
{
    private const string BASEURI = "http://localhost:5276";
    
    public async Task CreateUser(string userToken)
    {
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", userToken);
        var api = new Client(BASEURI, httpClient);

        await api.UserAsync(userToken);
    }
}