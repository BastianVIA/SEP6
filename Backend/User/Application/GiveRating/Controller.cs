using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.GiveRating;

[ApiController]
[Route("user")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }


  
    [HttpPut]
    [Route("rating")]
    [Tags("UserApi")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] SetRatingRequest request)
    {

        return Ok();
    }
    
    public class SetRatingRequest
    {
        [Required]
        public string MovieId { get; set; }

        [Required]
        public int Rating { get; set; }
    }

}