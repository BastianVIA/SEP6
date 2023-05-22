﻿using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.GetDetails;

[ApiController]
[Route("movie")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailsOfMoveFromId(string id)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];

        var query = new Query(id, userid);
        try
        {
            var result = _mediator.Send(query);
        
            return Ok(await result);

        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}