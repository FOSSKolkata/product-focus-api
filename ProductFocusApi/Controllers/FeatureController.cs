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

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class FeatureController : ControllerBase
    {
        private readonly Messages _messages;
        
        
        public FeatureController(
            Messages messages)
        {
            _messages = messages;
        }


        [HttpPut]
        public async Task<IActionResult> ModifyFeatureElement([FromBody] UpdateFeatureDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var command = new UpdateFeatureCommand(dto, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertScrumComment(UpsertScrumCommentCommand command)
        {
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertScrumWorkCompletionPercentage(UpsertScrumWorkCompletionPercentageCommand command)
        {
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }


        [HttpGet("{orgid}/{id}")]
        public async Task<IActionResult> GetFeatureDetailsById(long orgid, long id)
        {
            GetFeatureDetailsDto featureDetails = await _messages.Dispatch(new GetFeatureDetailsQuery(orgid, id));
            return Ok(featureDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeaturesOrdering([FromBody] OrderingInfoDto dto)
        {
            var command = new UpdateFeatureOrderingCommand(dto);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{prodId}/{sprintId}/{category}")]
        public async Task<IActionResult> GetFeatureOrderingByProductIdAndCategory(long prodId, long sprintId)
        {
            List<FeatureOrderDto> featureOrder = await _messages.Dispatch(new GetFeatureOrderingByProductIdAndCategoryQuery(prodId, sprintId));
            return Ok(featureOrder);
        }
    }
}
