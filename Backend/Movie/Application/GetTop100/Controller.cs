using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetTop100;

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("top100")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieTop100Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var query = new Query();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}