using BusinessRequirements.CommandHandlers;
using BusinessRequirements.CommandHandlers.Dtos;
using BusinessRequirements.QueryHandlers;
using BusinessRequirements.QueryHandlers.Dtos;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusinessRequirements.Application.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class BusinessRequirementController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BusinessRequirementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddBusinessRequirement([FromBody] AddBusinessRequirementDto dto)
        {
            var command = new AddBusinessRequirementCommand(dto.Title, dto.ProductId, dto.ReceivedOn, dto.TagIds, dto.SourceEnum, dto.SourceAdditionalInformation, dto.Description);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(command.Id) : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessRequirement(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteBusinessRequirementCommand(id, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessRequirementDetails(long id)
        {
            GetBusinessRequirementDetailsDto businessRequirementDetails = await _mediator.Send(new GetBusinessRequirementDetailsQuery(id));
            return Ok(businessRequirementDetails);
        }

        [HttpGet("{productId}/{offset}/{count}/query")]
        public async Task<IActionResult> GetBusinessRequirementsByProductId(long productId, [FromQuery] List<long> tagIds, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, long offset, long count)
        {
            GetBusinessRequirementsDto businessRequirements = await _mediator.Send(new GetBusinessRequirementListQuery(productId, tagIds, startDate, endDate, offset, count));
            return Ok(businessRequirements);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBusinessRequirement([FromBody] UpdateBusinessRequirementDto dto)
        {
            var command = await _mediator.Send(new UpdateBusinessRequirementCommand(dto.Id, dto.Title,
                dto.ReceivedOn, dto.TagIds, dto.SourceEnum, dto.SourceAdditionalInformation, dto.Description));
            return command.IsSuccess ? Ok() : BadRequest(command.Error);
        }

        [HttpPost("{businessRequirementId}")]
        public async Task<IActionResult> AddAttachments(long businessRequirementId)
        {
            var attachments = Request.Form.Files;
            var command = await _mediator.Send(new AddBusinessRequirementAttachmentCommand(businessRequirementId, attachments));
            return command.IsSuccess ? Ok() : BadRequest(command.Error);
        }

        [HttpGet("{businessRequirementId}")]
        public async Task<IActionResult> GetAttachmentsByBusinessRequirementId(long businessRequirementId)
        {
            List<GetBusinessRequirementAttachmentDto> attachments = await _mediator.Send(new GetBusinessRequirementAttachmentQuery(businessRequirementId));
            return Ok(attachments);
        }

        [HttpDelete("{businessRequirementId}/{attachmentId}")]
        public async Task<IActionResult> DeleteBusinessRequirementAttachment(long businessRequirementId, long attachmentId)
        {
            var command = await _mediator.Send(new DeleteBusinessRequirementAttachmentCommand(businessRequirementId, attachmentId));
            return command.IsSuccess ? Ok() : BadRequest(command.Error);
        }
    }
}