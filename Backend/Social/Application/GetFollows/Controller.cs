using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.GetFollows;
[ApiController]
[Route("Social")]
public class Controller: ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Gets the Id of all users the specified user follows
    /// </summary>
    /// <param name="userId">Id to specify user</param>
    /// <returns></returns>
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