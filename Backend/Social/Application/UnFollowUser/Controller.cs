﻿using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.UnFollowUser;

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
    /// UnFollow another user
    /// </summary>
    /// <param name="userIdToFollow"></param>
    /// <returns></returns>
    /// <remarks>This method requires authorization. Make sure to provide authorization when calling this method.</remarks>
    [HttpPut]
    [Route("unFollowUser")]
    [Tags("Social")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] string userIdToUnFollow)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token for user provided");
        }

        await _mediator.Send(new Command(userid, userIdToUnFollow));
        return Ok();
    }
}