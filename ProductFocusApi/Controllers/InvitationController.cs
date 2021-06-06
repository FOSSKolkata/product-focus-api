using Common;
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
       
        [HttpPost]
        public async Task<IActionResult> SendInvitation([FromBody] SendInvitationDto dto)
        {            
            var command = new SendInvitationCommand(dto.Email);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
