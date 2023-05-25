using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using Backend.Social.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.ReactToPost;

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
    /// React to post
    /// </summary>
    /// <param name="request">Reaction</param>
    /// <returns></returns>
    /// <remarks>This method requires authorization. Make sure to provide authorization when calling this method.</remarks>
    [HttpPut]
    [Route("reactToPost")]
    [Tags("Social")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] ReactToPostRequest request)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token for user provided");
        }

        await _mediator.Send(new Command(request.PostId, userid, request.Reation));
        return Ok();
    }
    
    public class ReactToPostRequest
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        public TypeOfReaction Reation { get; set; }
    }
}