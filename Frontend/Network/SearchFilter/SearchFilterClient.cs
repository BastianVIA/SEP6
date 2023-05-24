namespace Frontend.Network.SearchFilter;

public class SearchFilterClient : NSwagBaseClient, ISearchFilterClient
{
    public async Task<Entities.SearchFilter> GetSearchFilter(string searchTerm)
    {
        var response = await _api.SearchAsync(searchTerm);
        return new Entities.SearchFilter
        {
            numberOfPersons = response.PeopleResults,
            numberOfUsers = response.UserResults,
            numberOfMovies = response.MovieResults
        };
    }
}