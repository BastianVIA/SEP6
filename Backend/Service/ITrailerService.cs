namespace Backend.Service;

public interface ITrailerService
{
    public Task<string?>  GetMovieTrailerAsync(string moveId);
}