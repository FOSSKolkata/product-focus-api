﻿using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
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
            var command = new SendInvitationCommand(dto.OrgId, dto.Email);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationDto dto)
        {
            var command = new AcceptInvitationCommand(dto.InvitationId, dto.OrgId, dto.Email);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> RejectInvitation([FromBody] RejectInvitationDto dto)
        {
            var command = new RejectInvitationCommand(dto.InvitationId, dto.OrgId, dto.Email);
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
    }
}
