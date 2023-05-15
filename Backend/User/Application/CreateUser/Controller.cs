using System.ComponentModel.DataAnnotations;
using Backend.Movie.Application.Search;
using FirebaseAdmin.Auth;
using MediatR;
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
    public IActionResult Post([FromBody]string userId)
    {
        var command = new Command(userId);
        var result = _mediator.Send(command);

        return Ok(result);
    }
}