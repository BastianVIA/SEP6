using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.Details;

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
    [Route("{id}")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailsOfMoveFromId(string id)
    {
        var query = new Query(id);
        try
        {
            var result = _mediator.Send(query);
        
            return Ok(await result);

        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}