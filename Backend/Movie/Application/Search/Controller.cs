using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.Search;

[ApiController]
[Route("movie")]

public class Controller: ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("search")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string title)
    {
        var query = new Query(title);
        var result = _mediator.Send(query);
   
        return Ok(await result);
    }
}