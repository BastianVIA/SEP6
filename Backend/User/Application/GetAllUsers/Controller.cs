using Backend.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.User.Application.GetAllUsers;

[ApiController]
[Route("user")]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Searching for users on their name
    /// </summary>
    /// <param name="userSortingKey"></param>
    /// <param name="sortingDirection"></param>
    /// <param name="pageNumber">The number of movies per. page can be set in the configuration with "UsersPerPage"</param>
    /// <returns></returns>
    [HttpGet]
    [Route("getAllUsers")]
    [Tags("User")]
    [ProducesResponseType(typeof(SearchUser.UserSearchResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAll(UserSortingKey userSortingKey = UserSortingKey.DisplayName,
        SortingDirection sortingDirection = SortingDirection.DESC, int pageNumber = 1)
    {
        var query = new SearchUser.Query("", userSortingKey, sortingDirection,
            pageNumber); 
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
