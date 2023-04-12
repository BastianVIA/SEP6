using System.Collections;

namespace Backend.Service;

public class ServiceMovie
{
    public string Title;
    public int Id;
}
public interface IMovieService
{
    Task<List<ServiceMovie>> SearchForMovie(string keyWordSearch);
}