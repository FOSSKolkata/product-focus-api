using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class BusinessRequirementController : ControllerBase
    {
        private readonly Messages _messages;
        public BusinessRequirementController(Messages messages)
        {
            _messages = messages;
        }

        [HttpPost]
        public async Task<IActionResult> AddBusinessRequirement([FromBody] AddBusinessRequirementDto dto)
        {
            var command = new AddBusinessRequirementCommand(dto.Title, dto.ProductId, dto.ReceivedOn, dto.TagIds, dto.SourceEnum, dto.SourceAdditionalInformation, dto.Description);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok(command.Id) : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessRequirementDetails(long id)
        {
            GetBusinessRequirementDetailsDto businessRequirementDetails = await _messages.Dispatch(new GetBusinessRequirementDetailsQuery(id));
            return Ok(businessRequirementDetails);
        }

        [HttpGet("{productId}/{offset}/{count}/query")]
        public async Task<IActionResult> GetBusinessRequirementsByProductId(long productId, [FromQuery] List<long> tagIds, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, long offset, long count)
        {
            GetBusinessRequirementsDto businessRequirements = await _messages.Dispatch(new GetBusinessRequirementListQuery(productId, tagIds, startDate, endDate, offset, count));
            return Ok(businessRequirements);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBusinessRequirement([FromBody] UpdateBusinessRequirementDto dto)
        {
            var command = await _messages.Dispatch(new UpdateBusinessRequirementCommand(dto.Id, dto.Title,
                dto.ReceivedOn, dto.TagIds, dto.SourceEnum, dto.SourceAdditionalInformation, dto.Description));
            return command.IsSuccess ? Ok() : BadRequest(command.Error);
        }
    }
}
