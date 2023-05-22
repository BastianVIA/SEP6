using Backend.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.CreateReview;

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpPost]
    [Route("review")]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public IActionResult Post([FromBody] CreateReviewRequest reviewRequest)
    {
        Console.WriteLine("CREATE REVIEW ________________________________");
        var userid = (string?)HttpContext.Items[HttpContextKeys.UserId];
        if (userid == null)
        {
            return BadRequest("No token provided to create the user");
        }
        var command = new Command(userid, reviewRequest.MovieId, reviewRequest.ReviewBody);
        var result = _mediator.Send(command);
        return Ok();
    }

    public class CreateReviewRequest
    {
        public string MovieId { get; set; }
        public string ReviewBody { get; set; }
    }

}