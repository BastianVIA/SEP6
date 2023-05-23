using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetTop100;

[ApiController]
[Route("movie")]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("top")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieTop100Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int pageNumber = 1)
    {
        var query = new Query(pageNumber);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}