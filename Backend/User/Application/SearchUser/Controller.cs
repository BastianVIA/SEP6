using System.ComponentModel.DataAnnotations;
using Backend.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.SearchUser;

[ApiController]
[Route("user")]

public class Controller: ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Searching for users on their name
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="userSortingKey"></param>
    /// <param name="sortingDirection"></param>
    /// <param name="pageNumber">The number of movies per. page can be set in the configuration with "UsersPerPage"</param>
    /// <returns></returns>
    [HttpGet]
    [Route("search")]
    [Tags("User")]
    [ProducesResponseType(typeof(UserSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string displayName, UserSortingKey userSortingKey = UserSortingKey.DisplayName, SortingDirection sortingDirection = SortingDirection.DESC, int pageNumber = 1)
    {
        var query = new Query(displayName, userSortingKey, sortingDirection, pageNumber);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}