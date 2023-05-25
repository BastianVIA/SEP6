using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetTop;

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
    /// Get top movies
    /// </summary>
    /// <param name="pageNumber">The number of movies per. page can be set in the
    /// configuration with "MoviesPerPage"</param>
    /// <returns></returns>
    [HttpGet]
    [Route("top")]
    [Tags("Movie")]
    [ProducesResponseType(typeof(MovieTop100Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int pageNumber = 1)
    {
        var query = new Query(pageNumber);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}