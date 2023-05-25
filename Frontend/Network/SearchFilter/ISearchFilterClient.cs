namespace Frontend.Network.SearchFilter;

public interface ISearchFilterClient
{
    public Task<Entities.SearchFilter> GetSearchFilter(string searchTerm);
}