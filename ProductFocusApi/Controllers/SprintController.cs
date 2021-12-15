using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductFocusApi.CommandHandlers;
using Microsoft.AspNetCore.Authorization;
using ProductFocus.Dtos;

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
        public async Task<IActionResult> AddSprint([FromBody] AddSprintCommand command)
        {

            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSprintsByProductId(long id)
        {
            List<GetSprintDto> sprintList = await _messages.Dispatch(new GetSprintDetailsQuery(id));
            return Ok(sprintList);
        }
    }
}
