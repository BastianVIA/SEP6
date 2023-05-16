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


    [HttpGet]
    [Route("/recommendations")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieRecommendationsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetailsOfMoveFromId()
    {
        var query = new Query();
        var result = _mediator.Send(query);
        return Ok(await result);
    }
}