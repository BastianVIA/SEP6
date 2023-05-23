using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetDetails;

[ApiController]
[Route("movie")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets the details of a movie
    /// </summary>
    /// <param name="id">Internal Id of movie, Id is the same as it is on IMDB</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string id)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];

        var query = new Query(id, userid);
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