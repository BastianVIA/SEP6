using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.UserProfile;

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Gets details of the specified user
    /// </summary>
    /// <param name="userId">Id to specify user</param>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{userId}")]
    [Tags("UserApi")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required]string userId)
    {
        var query = new Query(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}