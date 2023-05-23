namespace Backend.Service;

public interface IResumeService
{
    Task<string?> GetResumeAsync(string id);
}