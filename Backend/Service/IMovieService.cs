using System.Collections;

namespace Backend.Service;

public class ServiceMovie
{
    public int Id;
    public string Title;
    public int ReleaseYear;
}
public interface IMovieService
{
    Task<List<ServiceMovie>> SearchForMovie(string keyWordSearch);
}