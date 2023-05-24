﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.SearchFilter.Application;

[ApiController]
[Route("filter")]

public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get the number of results for each search in the system
    /// </summary>
    /// <param name="searchTerm">The search term provided</param>
    /// <returns></returns>
    [HttpGet]
    [Route("search/{searchTerm}")]
    [Tags("Filter")]
    [ProducesResponseType(typeof(SearchFilterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([Required] string searchTerm)
    {
        var query = new Query(searchTerm);
        var result = _mediator.Send(query);

        return Ok(await result);
    }
}