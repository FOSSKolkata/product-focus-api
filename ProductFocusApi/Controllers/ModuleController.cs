using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ProductFocus.Domain.Common;
using MediatR;
using System.Collections.Generic;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddModule(long id, [FromBody] AddModuleDto dto)
        {
            var command = new AddModuleCommand(id, dto.Name);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModulesByProductId(long id)
        {
            List<GetModuleDto> moduleList = await _mediator.Send(new GetModuleListQuery(id));
            return Ok(moduleList);
        }
    }
}
