using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly Messages _messages;

        public UserController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            List<GetUserDto> userDetails = await _messages.Dispatch(new GetUserDetailsQuery(email));
            return Ok(userDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserListByOrganization(long id)
        {
            GetMemberOfOrganizationDto userDetails = await _messages.Dispatch(new GetUserListByOrganizationQuery(id));
            return Ok(userDetails);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {            
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            
            var command = new RegisterUserCommand(dto.Name, dto.Email, objectId);
            Result result = await _messages.Dispatch(command);
            
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }        
    }
}
