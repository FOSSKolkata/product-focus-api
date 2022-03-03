using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductFocusApi.CommandHandlers;
using Microsoft.AspNetCore.Authorization;
using ProductFocus.Dtos;
using ProductFocus.Domain.Common;
using MediatR;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class SprintController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SprintController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> AddSprint([FromBody] AddSprintCommand command)
        {

            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSprintsByProductId(long id)
        {
            List<GetSprintDto> sprintList = await _mediator.Send(new GetSprintDetailsQuery(id));
            return Ok(sprintList);
        }
    }
}
