using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestCaseCommands;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ProductTestCaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTestCaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddTestCase(AddTestCaseDto dto)
        {//Working
            var command = new AddTestCaseCommand(dto.Title, dto.Preconditions, dto.SuiteId, dto.TestSteps);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestCase(long id)
        {//Working
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteTestCaseCommand(id, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestCase(long id, [FromBody] UpdateTestCaseDto dto)
        {//Working
            var command = new UpdateTestCaseCommand(id, dto.Title, dto.Preconditions, dto.TestSteps);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> ReleaseTestCase(long id)
        {
            /*var command = new PublishTestCaseCommand(id);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);*/
            return Ok();
        }
    }
}
