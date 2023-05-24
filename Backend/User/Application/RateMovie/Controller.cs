using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.RateMovie;

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
    /// Update the users rating of the movie.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>This method requires authorization. Make sure to provide authorization when calling this method.</remarks>
    [HttpPut]
    [Route("rateMovie")]
    [Tags("User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] SetRatingRequest request)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token for user provided");
        }
        await _mediator.Send(new Command(userid, request.MovieId, request.Rating));

        return Ok();
    }
    
    public class SetRatingRequest
    {
        [Required]
        public string MovieId { get; set; }
        public int? Rating { get; set; }
    }

}