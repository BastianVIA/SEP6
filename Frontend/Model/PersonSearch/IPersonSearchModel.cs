using Frontend.Entities;

namespace Frontend.Model.PersonSearch;

public interface IPersonSearchModel
{
    Task<List<Entities.Person>> SearchForPersonAsync(string name, int? pageNumber = null);
}