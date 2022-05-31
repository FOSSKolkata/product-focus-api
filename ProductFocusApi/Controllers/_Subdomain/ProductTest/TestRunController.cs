using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestRunCommands;
using ProductTests.Application.QueryHandler.GetTestRunQueries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class TestRunController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TestRunController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("{testPlanId}")]
        public async Task<IActionResult> CreateTestRun(long testPlanId)
        {
            var command = new CreateTestRunCommand(testPlanId);
            Result<long> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTestRunsByProductId(long productId)
        {
            var command = new GetTestRunsQuery(productId);
            Result<List<GetTestRunsDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestRunById(long id)
        {
            var command = new GetTestRunByIdQuery(id);
            Result<GetTestRunDto> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
        [HttpPost]
        public async Task<IActionResult> MarkTestCasesVersion(List<MarkTestCaseVersionDto> testCases)
        {
            var command = new MarkTestCasesVersionCommand(testCases);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> MarkTestCaseVersionStatus(MarkTestCaseVersionStatus dto)
        {
            var command = new MarkTestCaseVersionStatusCommand(dto.Id, dto.ResultStatus);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{testSuiteId}")]
        public async Task<IActionResult> MarkTestStepVersionStatus(long testSuiteId, MarkTestStepVersionStatus dto)
        {
            var command = new MarkTestStepVersionStatusCommand(testSuiteId, dto.Id, dto.ResultStatus);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
