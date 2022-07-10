using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestCaseCommands;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductTests.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
[Authorize]
public class TestCaseController : ControllerBase
{
    private readonly IMediator _mediator;
    public TestCaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddTestCase([FromBody] AddTestCaseDto dto)
    {
        var command = new AddTestCaseCommand(dto.Title, dto.Preconditions,dto.TestPlanId, dto.TestSuiteId, dto.TestSteps);
        Result result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTestCase([FromBody] DeleteTestCaseDto dto)
    {
        string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var command = new DeleteTestCaseCommand(dto.Id, dto.TestPlanId, dto.TestSuiteId, objectId);
        Result result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTestCase(long id, [FromBody] UpdateTestCaseDto dto)
    {
        string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var command = new UpdateTestCaseCommand(id, dto.Title, dto.Preconditions, dto.TestSteps, objectId);
        Result result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
