using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProductFocusApi.CommandHandlers;
using System.Linq;
using System.Security.Claims;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using ProductFocus.Domain.Common;
using MediatR;
using System;
using ProductFocus.Domain.Model;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class FeatureController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public FeatureController(
            IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPut]
        public async Task<IActionResult> ModifyFeatureElement([FromBody] UpdateFeatureDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var command = new UpdateFeatureCommand(dto, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertScrumComment(UpsertScrumCommentCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpsertScrumWorkCompletionPercentage(UpsertScrumWorkCompletionPercentageCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }


        [HttpGet("{orgid}/{id}")]
        public async Task<IActionResult> GetFeatureDetailsById(long orgid, long id)
        {
            GetFeatureDetailsDto featureDetails = await _mediator.Send(new GetFeatureDetailsQuery(orgid, id));
            return Ok(featureDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeaturesOrdering([FromBody] OrderingInfoDto dto)
        {
            var command = new UpdateFeatureOrderingCommand(dto);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{prodId}/{sprintId}/{category}")]
        public async Task<IActionResult> GetFeatureOrderingByProductIdAndCategory(long prodId, long sprintId)
        {
            List<FeatureOrderDto> featureOrder = await _mediator.Send(new GetFeatureOrderingByProductIdAndCategoryQuery(prodId, sprintId));
            return Ok(featureOrder);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetFeatureListByProductId(long productId)
        {
            List<GetFeatureDto> features = await _mediator.Send(new GetFeatureListByProductIdQuery(productId));
            return Ok(features);
        }
        [HttpPost("{productId}/{workItemId}")]
        public async Task<IActionResult> MarkWorkItemAsCurrentlyProgress(long productId, long workItemId)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new AddCurrentProgressWorkItemCommand(productId, workItemId, objectId);
            Result<GetCurrentProgressWorkItemDto> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetCurrentProgressWorkItemId(long productId)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var query = new GetCurrentProgressWorkItemIdQuery(productId, objectId);
            Result<GetCurrentProgressWorkItemDto> result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetMyWorkItemsByProductId(long productId)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var query = new GetMyWorkItemsByProductIdQuery(productId, objectId);
            Result<List<GetWorkItemDto>> result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
