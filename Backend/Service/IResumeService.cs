namespace Backend.Service;

public interface IResumeService
{
    Task<string?> GetResume(string id);
}