using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.Search;

[ApiController]

public class Controller: ControllerBase
{
    private IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("Search")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string title)
    {
        var query = new Query(title);
        var result = await _mediator.Send(query);
   
        return Ok(result);
    }
}