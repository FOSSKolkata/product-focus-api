using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using ProductFocusApi.QueryHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.CommandHandlers;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class InvitationController : ControllerBase
    {
        private readonly Messages _messages;

        public InvitationController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet("{orgid}/{offset}/{count}")]
        public async Task<IActionResult> GetPendingInvitationList(long orgid, int offset, int count)
        {
            GetPendingInvitationDto pendingInvitationList = await _messages.Dispatch(new GetPendingInvitationListQuery(orgid, offset, count));
            return Ok(pendingInvitationList);
        }

        [HttpGet("{orgid}/{offset}/{count}")]
        public async Task<IActionResult> GetClosedInvitationList(long orgid, int offset, int count)
        {
            GetClosedInvitationDto closedInvitationList = await _messages.Dispatch(new GetClosedInvitationListQuery(orgid, offset, count));
            return Ok(closedInvitationList);
        }

        [HttpGet("{orgid}")]
        public async Task<IActionResult> GetUserListNotPartOfOrganization(long orgid)
        {
            List<GetUserNotPartOfOrgDto> userList = await _messages.Dispatch(new GetUserListNotInOrganizationQuery(orgid));
            return Ok(userList);
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitation([FromBody] SendInvitationDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new SendInvitationCommand(dto.OrgId, dto.Email, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> ResendInvitation([FromBody] ResendInvitationCommand dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new ResendInvitationCommand(dto.InvitationId, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new AcceptInvitationCommand(dto.InvitationId, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> RejectInvitation([FromBody] RejectInvitationDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new RejectInvitationCommand(dto.InvitationId, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CancelInvitation([FromBody] CancelInvitationDto dto)
        {
            var command = new CancelInvitationCommand(dto.InvitationId, dto.OrgId, dto.Email);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvitationDetailsById(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Result<GetInvitationDetailsDto> result = await _messages.Dispatch(new GetInvitationDetailsQuery(id, objectId));
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
