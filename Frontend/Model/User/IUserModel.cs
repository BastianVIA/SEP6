namespace Frontend.Model.User;

public interface IUserModel
{
    Task CreateUser(string userToken,string displayName, string email);
}