namespace Frontend.Model.SearchFilter;

public interface ISearchFilterModel
{
    public Task<Entities.SearchFilter> GetSearchFilter(string searchTerm);
}