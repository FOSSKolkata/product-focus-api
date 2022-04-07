using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestPlanCommands;
using ProductTests.Application.CommandHandler.TestSuiteCommands;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ProductTestSuiteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTestSuiteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddTestSuite(AddTestSuiteDto dto)
        {
            var command = new AddTestSuiteCommand(dto.Title, dto.TestPlanId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{planId}/{suiteId}")]
        public async Task<IActionResult> DeleteTestSuite(long planId, long suiteId)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteTestSuiteCommand(planId, suiteId, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
