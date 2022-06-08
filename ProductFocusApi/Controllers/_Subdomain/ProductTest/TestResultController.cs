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
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTestResults(long productId, string searchTitle, List<TestTypeEnum> searchTestTypes)
        {
            var command = new GetTestResultsQuery(productId, searchTitle, searchTestTypes);
            Result<List<GetTestResultsDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
