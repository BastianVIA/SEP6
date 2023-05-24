using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.People.Application.Search;

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
    /// Searching for person on their name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pageNumber">The number of movies per. page can be set in the
    /// configuration with "PeoplePerPage"</param>
    /// <returns></returns>
    [HttpGet]
    [Route("search")]
    [Tags("Person")]
    [ProducesResponseType(typeof(PersonSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Get([Required] string name, int pageNumber = 1)
    {
        var query = new Query(name, pageNumber);
        var result = _mediater.Send(query);

        return Ok(await result);
    }
}