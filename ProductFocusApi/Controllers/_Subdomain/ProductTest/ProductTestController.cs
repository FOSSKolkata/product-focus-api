using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestCaseCommands;
using ProductTests.Application.CommandHandler.TestPlanCommands;
using ProductTests.Application.CommandHandler.TestSuiteCommands;
using ProductTests.Application.QueryHandler.GetTestPlanQueries;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ProductTestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddTestPlan(AddTestPlanDto dto)
        {//Working
            var command = new AddTestPlanCommand(dto.ProductId, dto.SprintId, dto.Title, dto.TestType, dto.ProductDocumentationId, dto.WorkItemId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestPlan(long id)
        {//Working
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteTestPlanCommand(id, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTestPlans(long productId)
        {//Working
            var command = new GetTestPlansQuery(productId);
            Result<List<GetTestPlansDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{id}/{productId}")]
        public async Task<IActionResult> GetTestPlanDetails(long id, long productId)
        {//Working
            var command = new GetTestPlanDetailsQuery(productId, id);
            Result<GetTestPlanDetailsDto> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> AddTestSuite(AddTestSuiteDto dto)
        {//Working
            var command = new AddTestSuiteCommand(dto.Title, dto.TestPlanId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{planId}/{suiteId}")]
        public async Task<IActionResult> DeleteTestSuite(long planId, long suiteId)
        {//Working
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteTestSuiteCommand(planId, suiteId, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
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
    }
}
