using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ProductFocus.Domain.Common;
using MediatR;
// IdentityAndAccessManagement
namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            List<GetUserDto> userDetails = await _mediator.Send(new GetUserDetailsQuery(email));
            return Ok(userDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserListByOrganization(long id)
        {
            GetMemberOfOrganizationDto userDetails = await _mediator.Send(new GetUserListByOrganizationQuery(id));
            return Ok(userDetails);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {            
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var command = new RegisterUserCommand(dto.Name, dto.Email, objectId);
            Result result = await _mediator.Send(command);
            
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }        
    }
}
