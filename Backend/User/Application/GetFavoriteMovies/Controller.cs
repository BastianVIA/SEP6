using System.Diagnostics;
using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Backend.User.Application.GetFavoriteMovies;

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
    /// Get the specified users favorite movies
    /// </summary>
    /// <param name="userId">Id to specify user</param>
    /// <returns></returns>
    [HttpGet]
    [Route("favorite/{userId}")]
    [Tags("User")]
    [ProducesResponseType(typeof(FavoriteMovesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string userId)
    {
        var query = new Query(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    }
