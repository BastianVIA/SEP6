using Frontend.Entities;

namespace Frontend.Model.Recommendations;

public interface IRecommendationsModel
{
    public Task<List<Movie>> GetRecommendations();
}