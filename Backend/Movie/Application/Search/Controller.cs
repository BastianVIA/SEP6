using System.ComponentModel.DataAnnotations;
using Backend.Enum;
using Backend.Middleware;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Search for movie on the movies title
    /// </summary>
    /// <param name="title"></param>
    /// <param name="movieSortingKey"></param>
    /// <param name="sortingDirection"></param>
    /// <param name="pageNumber">The number of movies per. page can be set in the
    /// configuration with "MoviesPerPage"</param>
    /// /// <returns></returns>
    [HttpGet]
    [Route("search")]
    [Tags("Movie")]
    [ProducesResponseType(typeof(MovieSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string title, MovieSortingKey movieSortingKey = MovieSortingKey.Votes, SortingDirection sortingDirection = SortingDirection.DESC, int pageNumber = 1)
    {
        var query = new Query(title, movieSortingKey, sortingDirection, pageNumber);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}