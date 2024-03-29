﻿using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTests.Application.CommandHandler.TestPlanCommands;
using ProductTests.Application.QueryHandler.GetTestPlanQueries;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductTests.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class TestPlanController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TestPlanController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddTestPlan(AddTestPlanDto dto)
        {
            var command = new AddTestPlanCommand(dto.ProductId, dto.SprintId, dto.Title, dto.TestType, dto.ProductDocumentationId, dto.WorkItemId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestPlan(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteTestPlanCommand(id, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTestPlans(long productId)
        {
            var command = new GetTestPlansQuery(productId);
            Result<List<GetTestPlansDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{id}/{productId}")]
        public async Task<IActionResult> GetTestPlanDetails(long id, long productId)
        {
            var command = new GetTestPlanDetailsQuery(productId, id);
            Result<GetTestPlanDetailsDto> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
