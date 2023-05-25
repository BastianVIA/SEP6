using Frontend.Entities;

namespace Frontend.Model.PersonSearch;

public interface IPersonSearchModel
{
    Task<(int NumberOfPage, List<Entities.Person> PersonList)> SearchForPersonAsync(string name, int? pageNumber = null);
}