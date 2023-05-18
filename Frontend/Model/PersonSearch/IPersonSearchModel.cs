using Frontend.Entities;

namespace Frontend.Model.PersonSearch;

public interface IPersonSearchModel
{
    Task<List<Person>> SearchForPersonAsync(string name, int? pageNumber = null);
}