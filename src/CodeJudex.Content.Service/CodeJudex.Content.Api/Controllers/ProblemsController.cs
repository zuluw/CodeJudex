using CodeJudex.Content.Application.Commands.CreateProblem;
using CodeJudex.Content.Application.Queries.GetProblemBySlug;
using CodeJudex.Content.Application.Queries.GetProblemsList;
using CodeJudex.Content.Application.DTOs.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeJudex.Content.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProblemsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all available programming challenges.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProblemListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProblemListDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemsListQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves detailed information about a specific challenge by its slug.
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ProblemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProblemResponseDto>> GetBySlug(string slug, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemBySlugQuery(slug), ct);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new programming challenge.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProblemCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetBySlug), new { slug = command.Title.ToLower() }, result);
    }
}