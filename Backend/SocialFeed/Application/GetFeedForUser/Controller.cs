using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.SocialFeed.Application.GetFeedForUser;

[ApiController]
[Route("SocialFeed")]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("userFeed")]
    [Tags("SocialFeed")]
    [ProducesResponseType(typeof(GetFeedForUserResponse), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetFeedForUser(int pageNumber = 1)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No user token provided");
        }
        
        var query = new Query(userid, pageNumber);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}