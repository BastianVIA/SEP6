using Backend.SocialFeed.Application.GetFollowing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.SocialFeed.Application.GetFollows;
[ApiController]
[Route("Social")]
public class Controller: ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("follows/{userId}")]
    [Tags("Social")]
    [ProducesResponseType(typeof(GetFollowingResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeedForUser(string userId)
    {
        var query = new Query(userId);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}