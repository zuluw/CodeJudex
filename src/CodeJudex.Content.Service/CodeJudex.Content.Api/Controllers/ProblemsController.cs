using CodeJudex.Content.Application.Commands.CreateProblem;
using CodeJudex.Content.Application.Commands.DeleteProblem;
using CodeJudex.Content.Application.Commands.UpdateProblem;
using CodeJudex.Content.Application.DTOs.Responses;
using CodeJudex.Content.Application.Queries.GetProblemBySlug;
using CodeJudex.Content.Application.Queries.GetProblemsList;
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

    /// <summary>
    /// Deletes a programming challenge.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteProblemCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Updates a programming challenge.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProblemCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        await mediator.Send(command);
        return NoContent();
    }
}