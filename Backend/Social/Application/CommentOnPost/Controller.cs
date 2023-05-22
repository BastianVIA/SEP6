using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Social.Application.CommentOnPost;

[ApiController]
[Route("Social")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut]
    [Route("commentOnPost")]
    [Tags("Social")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] CommentOnPostRequest request)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token for user provided");
        }

        await _mediator.Send(new Command(request.PostId, userid, request.Comment));
        return Ok();

    }
    
    public class CommentOnPostRequest
    {
        [Required]
        public string PostId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}