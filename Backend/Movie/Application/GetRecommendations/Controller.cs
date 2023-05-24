using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetRecommendations;

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
    /// Get the recommended movies. The recommendation is based on the movies rating
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/recommendations")]
    [Tags("Movie")]
    [ProducesResponseType(typeof(MovieRecommendationsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var query = new Query();
        var result = _mediator.Send(query);
        return Ok(await result);
    }
}