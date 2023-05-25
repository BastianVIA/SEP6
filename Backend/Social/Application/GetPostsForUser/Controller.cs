using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.GetPostsForUser;

[ApiController]
[Route("Social")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the social feed for a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    /// <remarks>Shares Response with "userFeed", same documentation applies</remarks>
    [HttpGet]
    [Route("post/{userId}")]
    [Tags("Social")]
    [ProducesResponseType(typeof(GetPostsForUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeedForUser(string userId, int pageNumber = 1)
    {
        var query = new Query(userId, pageNumber);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}