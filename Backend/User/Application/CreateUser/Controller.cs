﻿using System.ComponentModel.DataAnnotations;
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

    /// <summary>
    /// Create new user for system.
    /// The user must have been created in Firebase before calling this, and the token must match the user from firebase
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>This method requires authorization. Make sure to provide authorization when calling this method.</remarks>
    [HttpPost]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public IActionResult Post([FromBody] CreateUserRequest request)
    {
        try
        {
            var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
            if (userid == null)
            {
                return BadRequest("No token provided to create the user");
            }
            var command = new Command(userid, request.DisplayName, request.Email);
            var result = _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error {e.Message}");
        }

    }

    public class CreateUserRequest
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}