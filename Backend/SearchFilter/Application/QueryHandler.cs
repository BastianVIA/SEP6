using MediatR;

namespace Backend.SearchFilter.Application;

public record Query(string SearchTerm) : IRequest<SearchFilterResponse>;

public record SearchFilterResponse(int MovieResults, int UserResults, int PeopleResults);

public class QueryHandler : IRequestHandler<Query, SearchFilterResponse>
{
    private readonly IMediator _mediator;

    public QueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<SearchFilterResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var movieResults =
            await _mediator.Send(new Movie.Application.NumberOfResultsForSearch.Query(request.SearchTerm));
        var userResults = 
            await  _mediator.Send(new User.Application.NumberOfResultsForSearch.Query(request.SearchTerm));
        var peopleResults = 
            await  _mediator.Send(new People.Application.NumberOfResultsForSearch.Query(request.SearchTerm));

        return new SearchFilterResponse(movieResults, userResults, peopleResults);
    }
}