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
    public class ProductTestRunController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTestRunController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("{testPlanId}")]
        public async Task<IActionResult> CreateTestRun(long testPlanId)
        {
            var command = new CreateTestRunCommand(testPlanId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTestRunsByProductId(long productId)
        {
            var command = new GetTestRunsQuery(productId);
            Result<List<GetTestRunsDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
