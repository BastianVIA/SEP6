using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.GetUserImage;


public class Controller : ControllerBase
{
    
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    /// <summary>
    /// Get image for specified user.
    /// 
    /// </summary>
    /// <param name="userId">Id to specify user</param>
    /// <returns></returns>
    /// <remarks>
    /// A "UserImageDto" will be returned, it will contain either 
    /// a picture if specified, or if the user has not yet specified a picture it will contain null.</remarks>
    [HttpGet]
    [Route("userImage/{userId}")]
    [Tags("UserApi")]
    [ProducesResponseType(typeof(UserImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserImage([Required]string userId)
    {
        var query = new Query(userId);
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    

}