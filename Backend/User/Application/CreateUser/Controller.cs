using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using Backend.Movie.Application.Search;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.CreateUser;

[ApiController]
[Route("user")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public IActionResult Post()
    {
        try
        {
            var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
            if (userid == null)
            {
                return BadRequest("No token provided to create the user");
            }
            var command = new Command(userid);
            var result = _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error {e.Message}");
        }

    }
}