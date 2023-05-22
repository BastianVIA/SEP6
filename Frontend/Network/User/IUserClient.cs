namespace Frontend.Network.User;

public interface IUserClient
{
    public Task CreateUser(string userToken, string displayName, string email);
    
    public Task SetReview(string userToken, string movieId, string review);
}