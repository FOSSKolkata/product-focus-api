using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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


        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            var command = new RegisterUserCommand(dto.Name, dto.Email);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }        
    }
}
