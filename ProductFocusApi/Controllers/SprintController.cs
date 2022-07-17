using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductFocusApi.CommandHandlers;
using Microsoft.AspNetCore.Authorization;
using ProductFocus.Dtos;
using MediatR;
using System.Linq;
using System.Security.Claims;
using ProductFocusApi.Dtos.Sprint;
//Execution
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
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSprintsByProductId(long id)
        {
            List<GetSprintDto> sprintList = await _mediator.Send(new GetSprintDetailsQuery(id));
            return Ok(sprintList);
        }

        [HttpPut("{id}/{productId}")]
        public async Task<IActionResult> UpdateSprint(long id, long productId, [FromBody] UpdateSprintDto dto)
        {
            var command = new UpdateSprintCommand(id, productId, dto.Name, dto.StartDate, dto.EndDate);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSprint(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new DeleteSprintCommand(id, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
