using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class SprintController : ControllerBase
    {
        private readonly Messages _messages;

        public SprintController(Messages messages)
        {
            _messages = messages;
        }


        [HttpPost]
        public async Task<IActionResult> AddSprint([FromBody] AddSprintDto dto)
        {
            var command = new AddSprintCommand(dto.Name, dto.StartTime, dto.EndTime);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
