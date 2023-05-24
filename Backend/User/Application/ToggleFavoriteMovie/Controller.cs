using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using Backend.User.Application.SetFavoriteMovie;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Backend.User.Application.ToggleFavoriteMovie;

[ApiController]
[Route("user")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Toggles the users favorite status of the given movie.
    /// 
    /// </summary>
    /// <param name="movieId"></param>
    /// <returns></returns>
    /// <remarks>This method requires authorization. Make sure to provide authorization when calling this method. 
    /// 
    /// If the user has the movie on their favorite list they will no longer after this is called.
    /// If they do not have the movie on their favorite list it will be added
    /// </remarks>
    [HttpPut]
    [Route("favorite/{movieId}")]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([Required] string movieId)
    {
        try
        {
            var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
            if (userid == null)
            {
                return BadRequest("No user token provided");
            }
            var command = new Command(userid, movieId);
            await _mediator.Send(command);
        }
        catch (InvalidDataException e)
        {
            LogManager.GetCurrentClassLogger().Error(e.StackTrace);
            return StatusCode(400, e.Message);
        }
        catch (Exception e)
        {
            LogManager.GetCurrentClassLogger().Error(e.StackTrace);
            return StatusCode(500, e.Message);
        }

        return Ok();
    }
}