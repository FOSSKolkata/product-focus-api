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
    public class ModuleController : ControllerBase
    {
        private readonly Messages _messages;

        public ModuleController(Messages messages)
        {
            _messages = messages;
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> AddFeature(long id, [FromBody] AddFeatureDto dto)
        {
            var command = new AddFeatureCommand(id, dto.Title, dto.WorkItemType, dto.SprintId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }        
    }
}
