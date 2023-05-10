using System.ComponentModel.DataAnnotations;
using Backend.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Movie.Application.Search;

[ApiController]
[Route("movie")]

public class Controller: ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("search")]
    [Tags("MovieApi")]
    [ProducesResponseType(typeof(MovieSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string title, MovieSortingKey movieSortingKey = MovieSortingKey.Votes, SortingDirection sortingDirection = SortingDirection.DESC, int pageNumber = 1)
    {
        var query = new Query(title, movieSortingKey, sortingDirection, pageNumber);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}