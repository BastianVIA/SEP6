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

    [HttpGet]
    [Route("details")]
    [Tags("PersonApi")]
    [ProducesResponseType(typeof(GetPersonDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string personId)
    {
        var query = new Query(personId);
        var result = _mediater.Send(query);

        return Ok(await result);
    }
}