namespace Backend.Service;

public interface ITrailerService
{
    public Task<string?>  GetMovieTrailer(string moveId);
}