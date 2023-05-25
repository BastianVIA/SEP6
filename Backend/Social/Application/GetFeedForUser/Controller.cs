using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.GetFeedForUser;

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
    /// <param name="pageNumber">Specifies the page number for paginated results. Default value is 1.</param>
    /// <response code="200">The social feed data for the user.</response>
    /// <remarks>
    /// This method requires authorization. Make sure to provide authorization when calling this method.
    /// 
    /// The data optionally includes an "ActivityDataDto", and the fields filled out in this dto depend on the topic:
    /// - FavoriteMovie: No ActivityDataDto provided.
    /// - CreatedRating: ActivityDataDto provided with "MovieId" and "NewRating" fields.
    /// - UpdatedRating: ActivityDataDto provided with "MovieId", "NewRating", and "OldRating" fields.
    /// - RemovedRating: ActivityDataDto provided with "MovieId" and "OldRating" fields.
    /// - NewUser: No ActivityDataDto provided.
    /// - UnFavoriteMovie: No ActivityDataDto provided.
    /// </remarks>
    [HttpGet]
    [Route("userFeed")]
    [Tags("Social")]
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