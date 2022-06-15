using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.QueryHandler.GetTestResultQueries;
using ProductTests.Domain.Model.TestPlanAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class TestResultController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TestResultController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{testPlanId}")]
        public async Task<IActionResult> GetTestResultsByTestPlanId(long testPlanId, [FromQuery] string searchTitle, [FromQuery] List<TestTypeEnum> searchTestTypes)
        {
            var command = new GetTestResultsQuery(testPlanId, searchTitle, searchTestTypes);
            Result<List<GetTestResultDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
