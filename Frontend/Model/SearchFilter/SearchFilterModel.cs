using Frontend.Network.SearchFilter;

namespace Frontend.Model.SearchFilter;

public class SearchFilterModel : ISearchFilterModel
{
    private ISearchFilterClient _client;

    public SearchFilterModel()
    {
        _client = new SearchFilterClient();
    }

    public async Task<Entities.SearchFilter> GetSearchFilter(string searchTerm)
    {
        return await _client.GetSearchFilter(searchTerm);
    }
}