using Frontend.Entities;

namespace Frontend.Model.Top100;

public interface ITop100Model
{
    public Task<List<Movie>> GetTopList(int pageNumber);
}