using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.People.Application.GetDetails;

[ApiController]
[Route("person")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediater;

    public Controller(IMediator mediater)
    {
        _mediater = mediater;
    }

    /// <summary>
    /// Gets details of person based on Id
    /// </summary>
    /// <param name="personId">Internal Id of person, does not match IMDB</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{Id}")]
    [Tags("PersonApi")]
    [ProducesResponseType(typeof(GetPersonDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string Id)
    {
        var query = new Query(Id);
        var result = _mediater.Send(query);

        return Ok(await result);
    }
}