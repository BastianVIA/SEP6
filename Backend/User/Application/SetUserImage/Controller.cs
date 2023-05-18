using System.ComponentModel.DataAnnotations;
using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.SetUserImage;

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut]
    [Route("userImage")]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> UpdateUserImage([FromBody] SetUserImageRequest request)
    {
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token for user provided");
        }
        var command = new Command(userid, request.ImageData);
        await _mediator.Send(command);

        return Ok();
    }

    public class SetUserImageRequest
    {
        public byte[] ImageData { get; set; }
    }
    
    

}